using Entity.Models;

namespace BussinessLogic.Interface
{
    public interface ICartItemService
    {
        Task<CartItem?> GetCartItemByIdAsync(Guid Id);
        Task<CartItem?> GetCartItemByCartIdAsync(Guid CartId);
        Task<IEnumerable<CartItem>> GetAllCartItemsByCartIdAsync(Guid CartId);
        Task<int?> GetQuantityAsync(Guid CartId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
    }
}