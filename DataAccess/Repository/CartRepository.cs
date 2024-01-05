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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(Cart cart)
        {
            _dbContext.Update(cart);
        }
    }
}
