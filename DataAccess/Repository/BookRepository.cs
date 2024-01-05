using DataAccess.Data;
using DataAccess.Interface;
using Entity.Models;
using Entity.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookWebStoreDbContext dbContext) : base(dbContext)
        {
        }
        public async Task AddNewCategoryAsync(Book book, Guid[] categoriesId, ICategoryRepository categoryData)
        {
            book.Categories.Clear();
            foreach(var Id in categoriesId)
            {
                Category category = await categoryData.GetAsync(Id);
                if(category is not null)
                {
                    book.Categories.Add(category);
                }
            }
        }
        public void Update(Book book)
        {
            _dbContext.Update(book);
        }
    }
}
