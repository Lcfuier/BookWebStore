using Entity.Models;

namespace DataAccess.Interface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Update(Customer customer);
    }
}