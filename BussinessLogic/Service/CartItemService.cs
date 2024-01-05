using BussinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Interface;
using DataAccess.UnitOfWorld;
using Entity.Models;
using Entity.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _data;
        public CartItemService(IUnitOfWork data)
        {
            _data = data;
        }

        public async Task<CartItem?> GetCartItemByIdAsync(Guid Id)
        {
            return await _data.CartItem.GetByIdAsync(s => s.CartItemID.Equals(Id), includeProperties: "Book");
        }

        public async Task<CartItem?> GetCartItemByCartIdAsync(Guid CartId)
        {
            return await _data.CartItem.GetByIdAsync(s => s.CartId.Equals(CartId), includeProperties: "Book");
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsByCartIdAsync(Guid CartId)
        {
            return await _data.CartItem.GetAll(s => s.CartId.Equals(CartId), includeProperties: "Book");
        }

        public async Task<int?> GetQuantityAsync(Guid CartId)
        {
            IEnumerable<CartItem> cartItems = await _data.CartItem.GetAll(s => s.CartId.Equals(CartId));
            return cartItems.Count();
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            _data.CartItem.Add(cartItem);
            await _data.SaveAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _data.CartItem.Update(cartItem);
            await _data.SaveAsync();
        }

        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _data.CartItem.Remove(cartItem);
            await _data.SaveAsync();
        }
    }
}
