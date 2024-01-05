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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(Author author)
        {
            _dbContext.Update(author);
        }
    
    }
}
