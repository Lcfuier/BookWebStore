using DataAccess.Data;
using DataAccess.Interface;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(Category category)
        {
            _dbContext.Update(category);
        }
    }
}
