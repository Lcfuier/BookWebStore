using Entity.Models;

namespace DataAccess.Interface
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail item);
    }
}