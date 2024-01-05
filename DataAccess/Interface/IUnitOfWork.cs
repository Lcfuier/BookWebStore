using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IUnitOfWork :IDisposable
    {
        IBookRepository Book { get; }
        ICategoryRepository Category { get; }
        ICartItemRepository CartItem { get;  }
        ICartRepository Cart { get;  }
        IOrderDetailRepository OrderDetail { get; }
        IOrderRepository Order { get; }
        IAuthorRepository Author { get; }
        IPublisherRepository Publisher { get; }
        ICustomerRepository Customer { get; }
        Task SaveAsync();
    }
}
