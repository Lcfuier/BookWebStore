using BussinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Interface;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _data;
        private readonly BookWebStoreDbContext _context;
        public BookController(IBookService bookService, ICategoryService categoryService, IUnitOfWork data, BookWebStoreDbContext context)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _data = data;
            _context = context;
        }

        public async Task<IActionResult> Detail(Guid? id )
        {
            var book= await _bookService.GetBookByIdAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            CartItemDTO cartItemDto = new()
            {
                BookId = book.BookId,
                Quantity = 1

            };
            ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is not null) // user have login, retrieve the cart item quantity
            {
               var user = await _context.Users.FindAsync(claim.Value)
                    ?? throw new Exception("User not found.");

                Cart? cart = await _data.Cart.GetAsync(new QueryOptions<Cart>
                {
                    Where = c => c.customerId == user.Id
                });

                if (cart is not null)
                {
                    // first, it need to be exist in cart (belong to what cart)
                    // second, does it match the book we need to find in the cart?
                    CartItem? existingCartItem = await _data.CartItem.GetAsync(new QueryOptions<CartItem>
                    {
                        Where = ci => ci.CartId == cart.CartId && ci.BookId == book.BookId
                    });

                    // Already have one in db
                    if (existingCartItem is not null)
                    {
                        cartItemDto.CartId = existingCartItem.CartId;
                        cartItemDto.CartItemID = existingCartItem.CartItemID;
                        cartItemDto.Quantity= existingCartItem.Quantity;
                    }
                }
            }
            BookDetailsViewModel vm = new()
            {
                CartItemDTO = cartItemDto,
                Book = book
            };
            return View(vm);
        }
        public async Task<IActionResult> GetBookByAuthor(Guid id)
        {
            var result = await _bookService.GetBooksByAuthorId(id);
            return View(result);
        }
        public async Task<IActionResult> GetBookByCategory(Guid id)
        {
            var category= await _categoryService.GetCategoryByIdAsync(id);
            var books = await _bookService.GetBooksByCategory(id);
            BookCategoryDTO result = new BookCategoryDTO()
            {
                category=category,
                books=books
            };
            return View(result);
        }
    }
}
