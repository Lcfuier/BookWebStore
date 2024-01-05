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
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(BookWebStoreDbContext context) : base(context)
        {
        }

        public void Update(Publisher publisher)
        {
            _dbContext.Update(publisher);
        }
    }
}
