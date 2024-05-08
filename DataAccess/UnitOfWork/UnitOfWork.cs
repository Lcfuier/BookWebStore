using DataAccess.Data;
using DataAccess.Interface;
using DataAccess.Repository;
using Entity.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookWebStoreDbContext _context;
        public IBookRepository Book { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICartItemRepository CartItem { get; private set; }
        public IAuthorRepository Author { get; private set; }

        public IPublisherRepository Publisher { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public IOrderRepository Order { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public UnitOfWork(BookWebStoreDbContext context)
        {
            _context = context;
            Book = new BookRepository(_context);
            Category = new CategoryRepository(_context);
            CartItem = new CartItemRepository(_context);
            Cart = new CartRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            Order = new OrderRepository(_context);
            Author = new AuthorRepository(_context);
            Publisher = new PublisherRepository(_context);
            Customer = new CustomerRepository(_context);
        }
        
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
