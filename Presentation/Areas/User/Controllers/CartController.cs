using AutoMapper;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using DataAccess.Data;
using DataAccess.UnitOfWork;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Presentation.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles=Roles.User)]
    public class CartController : Controller
    {
        private readonly ICartItemService _cartItemService;
        private readonly ICartService _cartService;
        private readonly BookWebStoreDbContext _db;
        private readonly IMapper _mapper;
        public CartController(ICartItemService cartItemService, ICartService cartService, BookWebStoreDbContext db, IMapper mapper) 
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
            _db = db;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Customer.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");

            HttpContext.Session.SetInt32(SessionSD.CartItemQuantityKey, (await _cartService.GetTotalCartItemsCountAsync(user.Id)));

            Cart cart = await _cartService.GetCartByUserAsync(user.Id) ?? new Cart();

            cart.CartItems = (await _cartItemService.GetAllCartItemsByCartIdAsync(cart.CartId)).ToList();

            return View(cart);
        }
        [HttpGet]
        public async Task<IActionResult> AddCartItemForBookDetails(CartItemDTO cartItemDTO)
            {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Customer.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");

            await _cartService.AddCartItemAsync(user.Id, _mapper.Map<CartItem>(cartItemDTO));
            TempData["success"] = "Thêm giỏ hàng thành công !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveCartItem (Guid Id)
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            CartItem? cartItem = await _cartItemService.GetCartItemByIdAsync(Id);
            if (cartItem is null)
            {
                return NotFound();
            }
            await _cartService.RemoveCartItemAsync(user.Id, cartItem);
            TempData["success"] = "Xóa thành công!";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> PlusCartItem(Guid Id)
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            CartItem? cartItem = await _cartItemService.GetCartItemByIdAsync(Id);
            if (cartItem is null)
            {
                return NotFound();
            }
            await _cartService.Plus(user.Id, Id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MinusCartItem(Guid Id)
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            CartItem? cartItem = await _cartItemService.GetCartItemByIdAsync(Id);
            if (cartItem is null)
            {
                return NotFound();
            }
            await _cartService.Minus(user.Id, Id);
            return RedirectToAction(nameof(Index));
        }
        #region
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem(Guid Id)
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FindAsync(claim?.Value)
                ?? throw new Exception("User not found.");
            CartItem? cartItem = await _cartItemService.GetCartItemByIdAsync(Id);
            if (cartItem is null)
            {
                return Json(new { success = false, message = "Lỗi khi xóa!" });
            }
            await _cartService.RemoveCartItemAsync(user.Id, cartItem);
            return Json(new { success = true, message = "Xóa thành công!" });

        }
        #endregion
    }
}
