using BussinessLogic.Interface;
using DataAccess.Interface;
using Entity.Models;
using Entity.Query;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class CartService:  ICartService 
    {
        private readonly IUnitOfWork _data;
        public CartService(IUnitOfWork data)
        {
            _data = data;
        }   
        public async Task<IEnumerable<Cart>> GetAllCartAsync()
        {
            return await _data.Cart.GetAll();
        }
        public async Task<int> GetCountCartItemByIdAsync(string CustumerId)
        {
            var list = await _data.Cart.GetAll(c => c.customerId.Equals(CustumerId));
            var count = list.Count();
            if (count == 0)
                return 0;
            return count;
        }
        public Task<Cart?> GetCartByUserAsync(string userId)
        {
            return _data.Cart.GetByIdAsync(s=>s.customerId.Equals(userId));
        }
        public async Task AddCartItemAsync(string userId,CartItem item)
        {
            Cart? cart= await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.customerId == userId
            });
            if (cart is null)
            {
                cart=new Cart()
                {
                    customerId = userId,
                };
                await AddCartAsync(cart);
            }

            CartItem? existItem = await _data.CartItem.GetAsync(new QueryOptions<CartItem>
            {
                Includes = "Book",
                Where = ci => ci.CartItemID == item.CartItemID
            });
            if (existItem is null)
            {
                item.CartId = cart.CartId;
                //find
                Book? book = await _data.Book.GetAsync(item.BookId);
                if (book is not null)
                {
                    item.Book = book;
                    item.Price = (book.Price-book.DiscountPercent * book.Price) * item.Quantity;

                }
                cart.Amount += item.Price;
                _data.CartItem.Add(item);

            }
            else
            {
                var book = await _data.Book.GetAsync(item.BookId);
                if (item.Quantity > 0)
                {
                    cart.Amount -= existItem.Price;
                    existItem.Quantity += item.Quantity;
                    existItem.Price = (book.Price - book.DiscountPercent * book.Price) * existItem.Quantity;
                    cart.Amount+= existItem.Price;       

                }
                else
                {
                    cart.Amount-=existItem.Price;
                    _data.CartItem.Remove(existItem);
                }
            }
            await _data.SaveAsync();
        }

        public async Task RemoveCartItemAsync(string customerId, CartItem cartItem)
        {
            Cart cart = await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.customerId == customerId
            }) ?? throw new Exception("Cart not found.");

            cart.Amount -= cartItem.Price;
            _data.CartItem.Remove(cartItem);
            await _data.SaveAsync();
        }
        public async Task Plus(string customerId,Guid CartItemId)
        {
            Cart cart = await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.customerId == customerId
            }) ?? throw new Exception("Cart not found.");
            CartItem? existItem = await _data.CartItem.GetAsync(new QueryOptions<CartItem>
            {
                Includes = "Book",
                Where = ci => ci.CartItemID.Equals(CartItemId) && ci.CartId.Equals(cart.CartId)
            });
            cart.Amount-= existItem.Price;
            existItem.Quantity += 1;
            existItem.Price = (existItem.Book.Price - existItem.Book.DiscountPercent * existItem.Book.Price) * existItem.Quantity;
            cart.Amount += existItem.Price;
            await _data.SaveAsync();
        }
        public async Task Minus(string customerId, Guid CartItemId)
        {
            Cart cart = await _data.Cart.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.customerId == customerId
            }) ?? throw new Exception("Cart not found.");
            CartItem? existItem = await _data.CartItem.GetAsync(new QueryOptions<CartItem>
            {
                Includes = "Book",
                Where = ci => ci.CartItemID.Equals(CartItemId) && ci.CartId.Equals(cart.CartId)
            });
            cart.Amount -= existItem.Price;
            if (existItem.Quantity == 1)
            {
                await RemoveCartItemAsync(customerId, existItem);
            }
            else
            {
                existItem.Quantity -= 1;
                existItem.Price = (existItem.Book.Price - existItem.Book.DiscountPercent * existItem.Book.Price) * existItem.Quantity;
                cart.Amount += existItem.Price;
                await _data.SaveAsync();
            }
            
        }
        public async Task<int?> GetQuantityAsync(string customerId)
        {
            Cart? cart = await _data.Cart.GetByIdAsync(c => c.customerId.Equals(customerId), includeProperties: "CartItems");
            return cart?.CartItems.Count;
        }

        public async Task<int> GetTotalCartItemsCountAsync(string customerId)
        {
            Cart? cart = await _data.Cart.GetByIdAsync(c=>c.customerId.Equals(customerId),includeProperties:"CartItems");

            if (cart is null)
            {
                return 0;
            }

            int count = 0;
            foreach (CartItem cartItem in cart.CartItems)
            {
                count += cartItem.Quantity;
            }

            return count;
        }

        public async Task AddCartAsync(Cart cart)
        {
            _data.Cart.Add(cart);
            await _data.SaveAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _data.Cart.Update(cart);
            await _data.SaveAsync();
        }

        public async Task RemoveCartAsync(Cart cart)
        {
            _data.Cart.Remove(cart);
            await _data.SaveAsync();
        }
    }
}
