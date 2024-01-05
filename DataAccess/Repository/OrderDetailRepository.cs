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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(BookWebStoreDbContext context) : base(context)

        {
        }

        public void Update(OrderDetail orderDetail)
        {
            _dbContext.Update(orderDetail);
        }
    }
}
