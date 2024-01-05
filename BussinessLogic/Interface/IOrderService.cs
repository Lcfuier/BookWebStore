using Entity.Models;
using System.Threading.Tasks;

namespace BussinessLogic.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<Order?> GetOrderByCustomerIdAsync(string id);
        Task AddOrderAsync(Order order);
        Task UpdateStripePaymentId(Guid Id, string sessionId, string paymentIntentId);
        Task UpdateStatus(Guid Id, string? orderStatus, string? paymentStatus);
        Task UpdateOrderAsync(Order category);
        Task RemoveOrderAsync(Order category);
        Task<IEnumerable<Order>> GetAllOrderByUserId(string userId);
    }
}