using Entity.Models;

namespace DataAccess.Interface
{
    public interface IAuthorRepository : IRepository<Author>
    {
        void Update(Author author);
    }
}