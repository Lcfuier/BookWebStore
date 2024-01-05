using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface ICartRepository : IRepository<Cart>
    {
        void Update(Cart cart);
    }
}
