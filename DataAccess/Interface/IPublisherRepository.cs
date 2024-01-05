using DataAccess.Repository;
using Entity.Models;

namespace DataAccess.Interface
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        void Update(Publisher publisherCompany);
    }
}
