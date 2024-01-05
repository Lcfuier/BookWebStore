using Entity.Models;

namespace BussinessLogic.Interface
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartAsync();
        Task<Cart?> GetCartByUserAsync(string userId);
        Task AddCartItemAsync(string userId, CartItem item);
        Task<int> GetCountCartItemByIdAsync(string CustumerId);
        Task RemoveCartItemAsync(string customerId, CartItem cartItem);
        Task<int?> GetQuantityAsync(string customerId);
        Task<int> GetTotalCartItemsCountAsync(string customerId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task RemoveCartAsync(Cart cart);
        Task Plus(string customerId, Guid CartItemId);
        Task Minus(string customerId, Guid CartItemId);
    }
}