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
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(Customer customer)
        {
            _dbContext.Update(customer);
        }
    }
}
