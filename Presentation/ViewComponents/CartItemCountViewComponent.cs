using BussinessLogic.Interface;
using BussinessLogic.Service;
using DataAccess.Data;
using Entity.Constants;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.ViewComponents
{
    public class CartItemCountViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly BookWebStoreDbContext _db;
        public CartItemCountViewComponent(ICartService cartService, BookWebStoreDbContext db)
        {
            _cartService = cartService;
            _db= db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FindAsync(claim?.Value);
            if (claim == null)
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            else
            {
                if (HttpContext.Session.GetInt32(SessionSD.CartItemQuantityKey) == null)
                    HttpContext.Session.SetInt32(SessionSD.CartItemQuantityKey, (await _cartService.GetTotalCartItemsCountAsync(user.Id)));

                return View(HttpContext.Session.GetInt32(SessionSD.CartItemQuantityKey));
            }
        }
    }
}
