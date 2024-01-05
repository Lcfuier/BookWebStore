using AutoMapper;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using DataAccess.Data;
using DataAccess.Interface;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using Entity.Constants.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Reflection.Metadata;
using System.Security.Claims;
using Stripe.Checkout;
using System.Net.WebSockets;
using DataAccess.UnitOfWorld;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Presentation.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = Roles.User)]
    public class OrderController : Controller
    {
        private readonly BookWebStoreDbContext _db;
        private readonly IOrderService _OrderService;
        private readonly IUnitOfWork _data;
        private readonly IMapper _mapper;
        public OrderController(BookWebStoreDbContext db, IOrderService OrderService, IUnitOfWork data, IMapper mapper)
        {
            _db = db;
            _OrderService = OrderService;
            _data = data;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Payment(Guid id)
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Customer.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");

            Cart? cart = await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Includes = "CartItems.Book",
                Where = c => c.CartId.Equals(id)
            });
            if (cart is null)
            {
                return NotFound();
            }
            OrderDTO orderDTO = new()
            {
                Name = user.LastName + " " + user.LastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
            };
            OrderIndexViewModel vm = new()
            {
                OrderDto = orderDTO,
                CartDto = _mapper.Map<CartDTO>(cart),
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(OrderIndexViewModel vm)
        {

            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Customer.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            Cart? existingCart = await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Includes = "CartItems.Book",
                Where = c => c.CartId == vm.CartDto.CartId
            });
            var cartItems = existingCart.CartItems;
            if (existingCart is null && vm.CartItemDto.BookId.Equals(0))
            {
                throw new Exception("Cart and item not found.");
            }
            Order order = new()
            {
                Name = vm.OrderDto.Name,
                PhoneNumber = vm.OrderDto.PhoneNumber ?? string.Empty,
                OrderStatus = OrderStatus.StatusPending,
                PaymentStatus = paymentStatus.PaymentStatusPending,
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                TotalAmount = vm.CartDto.Amount,
                Address = vm.OrderDto.Address,
                Ward = vm.OrderDto.Ward,
                District = vm.OrderDto.District,
                City = vm.OrderDto.City,
                customerId = user.Id
            };
            await _OrderService.AddOrderAsync(order);
            if (existingCart is null)
            {
                OrderDetail orderDetail = new()
                {
                    BookId = vm.CartItemDto.BookId,
                    Quantity = vm.CartItemDto.Quantity,
                    Price = vm.CartItemDto.Price,
                    OrderId = order.OrderId
                };
                _data.OrderDetail.Add(orderDetail);
            }
            else
            {
                List<OrderDetail> orderDetails = new();
                foreach (CartItem cartItem in existingCart.CartItems)
                {
                    // Create new order detail
                    OrderDetail orderDetail = new()
                    {
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        OrderId = order.OrderId
                    };

                    orderDetails.Add(orderDetail);
                }
                _data.OrderDetail.AddRange(orderDetails);

                _data.Cart.Remove(existingCart);
            }
            await _data.SaveAsync();
            HttpContext.Session.SetInt32(SessionSD.CartItemQuantityKey, 0);
            //payment
            var domain = Request.Scheme + "://" + Request.Host.Value;
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{domain}/User/Order/OrderConfirmation?id={order.OrderId}",
                CancelUrl = $"{domain}/User/Cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in cartItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Book.PriceDiscount,
                        Currency = "VND",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Book.Title
                        },
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
           await _OrderService.UpdateStripePaymentId(order.OrderId, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            TempData["success"] = "Đặt hàng thành công !";
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> OrderConfirmation(Guid Id)
        {
            var  order = await _data.Order.GetAsync(Id);
            if (order.PaymentStatus.Equals(paymentStatus.PaymentStatusApproved))
            {
                return View(Id);
            }
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _OrderService.UpdateStripePaymentId(order.OrderId, session.Id, session.PaymentIntentId);
                await _OrderService.UpdateStatus(order.OrderId, OrderStatus.StatusApproved, paymentStatus.PaymentStatusApproved);
                return View(Id);
            }
            return RedirectToAction("Index", "User/Cart");
        }
        public async Task<IActionResult> OrderManager()
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Customer.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            var result= await _OrderService.GetAllOrderByUserId(user.Id);
            return View(result);
        }
    }
}
