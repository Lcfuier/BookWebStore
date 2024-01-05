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
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(CartItem item)
        {
            _dbContext.Update(item);
        }
    }
}
