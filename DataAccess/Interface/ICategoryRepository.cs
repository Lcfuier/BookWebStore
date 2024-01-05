using Entity.Models;

namespace DataAccess.Interface
{
    public interface ICategoryRepository: IRepository<Category> 
    {
        void Update(Category category);
    }
}