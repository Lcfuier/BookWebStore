using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IBookRepository :IRepository<Book>
    {
        Task AddNewCategoryAsync(Book book, Guid[] categoriesId, ICategoryRepository categoryData);
        void Update(Book book);
    }
}
