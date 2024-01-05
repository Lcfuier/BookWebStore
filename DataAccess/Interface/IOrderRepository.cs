using Entity.Models;

namespace DataAccess.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}