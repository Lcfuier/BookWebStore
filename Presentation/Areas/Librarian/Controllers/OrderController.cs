using AutoMapper;
using BussinessLogic.Interface;
using DataAccess.Interface;
using Entity.Constants;
using Entity.Constants.Status;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using System.Security.Claims;

namespace Presentation.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Entity.Models.Customer> _userManager;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        public OrderController(IUnitOfWork unitOfWork, UserManager<Entity.Models.Customer> userManager, IMapper mapper, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(Guid id) // order id
        {
            OrderDTO orderDto = new OrderDTO();
            var allOrderDetails = await _unitOfWork.OrderDetail.ListAllAsync(new QueryOptions<OrderDetail>()
            {
                Where = b => b.OrderId.Equals(id),
                Includes = "Book"
            });
            

            var selectedOrder  = await _unitOfWork.Order.GetAsync(new QueryOptions<Order>()
            {
                Where = b => b.OrderId.Equals(id),
                Includes = "OrderDetails,Customer"
            });
            if (selectedOrder is not null)
            {
                orderDto = _mapper.Map<OrderDTO>(selectedOrder);
            }

            if (selectedOrder is null)
                return NotFound();
            return View(orderDto);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        public async Task<IActionResult> UpdateOrderDetails(OrderDTO orderDto)
        {
            if (orderDto is null)
            {
                return NotFound();
            }
            // Update Order
            var order = await _unitOfWork.Order.GetAsync(orderDto.OrderId);
            if (order is null)
            {
                return NotFound();
            }
            order.Name = orderDto.Name;
            order.PhoneNumber = orderDto.PhoneNumber;
            order.Address = orderDto.Address;
            order.Ward = orderDto.Ward;
            order.District = orderDto.District;
            order.City= orderDto.City;
            order.Address = orderDto.Address;
            order.Carrier = orderDto.Carrier;
            order.TrackingNumber= orderDto.TrackingNumber;
              _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();
            TempData["success"] = "Cập nhật thành công !";
            return RedirectToAction(nameof(Details), new { id = order.OrderId});
        }

        [HttpPost]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        public async Task<IActionResult> StartProcessing(OrderDTO orderDto)
        {
            if (orderDto is null)
            {
                return NotFound();
            }
            var order = await _orderService.GetOrderByIdAsync(orderDto.OrderId);
            if (order is null)
            {
                return NotFound();
            }
            order.OrderStatus = OrderStatus.StatusInProcess;
            await _orderService.UpdateStatus(order.OrderId, OrderStatus.StatusInProcess,null);
            TempData["success"] = "Đơn hàng đang được xử lý !";
            return RedirectToAction(nameof(Details), new { id = order.OrderId});
        }

        [HttpPost]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        public async Task<IActionResult> ShipPartialOrder(OrderDTO orderDto)
        {
            if (orderDto is null)
            {
                return NotFound();
            }
            var order = await _orderService.GetOrderByIdAsync(orderDto.OrderId);
            if (order is null)
            {
                return NotFound();
            }
            order.OrderStatus = OrderStatus.StatusShipped;
            order.ShippingDate = DateTime.Now;
            await _orderService.UpdateStatus(order.OrderId, OrderStatus.StatusShipped, null);
            TempData["success"] = "Đơn hàng đã được vận chuyển !";
            return RedirectToAction(nameof(Details), new { id = order.OrderId });
            //
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> CompleteOrder(OrderDTO orderDto)
        {
            var order = await _orderService.GetOrderByIdAsync(orderDto.OrderId);
            if(order is null)
            {
                return NotFound();
            }
            else
            {
                await _orderService.UpdateStatus(order.OrderId, OrderStatus.StatusCompleted,null);
            }
            

            TempData["success"] = "Đơn hàng đã hoàn thành !";
            return RedirectToAction(nameof(Details), new { id = order.OrderId });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        public async Task<IActionResult> CancelOrder(OrderDTO orderDto)
        {
            var order = await _unitOfWork.Order.GetAsync(new QueryOptions<Order>()
            {
                Where=r=>r.OrderId.Equals(orderDto.OrderId),
                Includes="Customer"
            });
            if (order is null)
            {
                return NotFound();
            }
            if (!order.OrderStatus.CanBeCancelled())
            {
                TempData["error"] = "Không thể hủy đơn hàng !";
                return RedirectToAction(nameof(Details), new { id = order.OrderId});
            }
            if (order.PaymentStatus == paymentStatus.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntentId,
                };
                var service = new RefundService();
                service.Create(options);
                await _orderService.UpdateStatus(order.OrderId, OrderStatus.StatusCancelled, paymentStatus.PaymentStatusRefunded);

            }
            else
            {
              await _orderService.UpdateStatus(order.OrderId, OrderStatus.StatusCancelled, paymentStatus.PaymentStatusCancelled);
            }
            TempData["success"] = "Đơn hàng đã hủy thành công !";
            return RedirectToAction(nameof(Details), new { id = order.OrderId});
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll(string? status)
        {
            IEnumerable<Order> data;

            // Default role filter is empty
            Func<Order, bool> roleFilter = r => false;

            if (string.IsNullOrEmpty(status) || status == "All" || status == "null")
            {
                data = await _unitOfWork.Order.ListAllAsync(new QueryOptions<Order>()
                {
                    Includes = "OrderDetails.Book,Customer",
                });
            }
            else

            {
                data = await _unitOfWork.Order.ListAllAsync(new QueryOptions<Order>()
                {
                    Where = r=>r.OrderStatus.Equals(status),
                    Includes = "OrderDetails.Book,Customer",
                });
            }


            if (User.IsInRole(Roles.Librarian) || User.IsInRole(Roles.Admin))
            {
                // Accept all
                roleFilter = r => true;
            }

            else if (User.IsInRole(Roles.User))
            {
                // Accept only orders that belong to the current user
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                roleFilter = r => r.customerId.Equals(userId);
            }

            data = data.Where(roleFilter);
            return Json(new { data });
        }

        #endregion
    }
}

