using BookWebStore.UnitTest.Mocks;
using DataAccess.Data;
using DataAccess.Repository;
using Entity.Models;
using Entity.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWebStore.UnitTest.Repositories
{
    public class CartRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var Cart1 = new Cart
            {
                CartId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                Amount=(decimal)300000,
                customerId="123",
                CartItems=new List<CartItem> {new CartItem { CartId = new Guid()} }
                
            };
            var Cart2 = new Cart
            {
                CartId = new Guid("6281f912-aa12-45af-9bfa-61472d874698"),
                Amount = (decimal)300000,
                customerId = "12334",
                CartItems = new List<CartItem> { new CartItem { CartId = new Guid() } }
            };
            var Cart3 = new Cart
            {
                CartId = new Guid("2589da73-6063-434a-947b-9336095d863c"),
                Amount = (decimal)300000,
                customerId = "12312",
                CartItems = new List<CartItem> { new CartItem { CartId = new Guid() } }
            };
            await context.Cart.AddAsync(Cart1);
            await context.Cart.AddAsync(Cart3);
            await context.Cart.AddAsync(Cart2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetCartsAsync_WhenSuccessful_ShouldReturnCarts()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new CartRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Cart>>(actual);
            Assert.Equal(context.Cart.Count(), actual.Count());
        }
        [Fact]
        public async Task GetCartsByIdAsync_WhenSuccessful_ShouldReturnCart()
        {
            // Arrange
            var id = new Guid("6281f912-aa12-45af-9bfa-61472d874698");
            var context = await SeedDatabaseContext();
            var sut = new CartRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.CartId == id
            });

            // Assert
            Assert.IsType<Cart>(actual);
        }
        [Fact]
        public async Task AddCartAsync_WhenSuccessful_ShouldAddCart()
        {
            // Arrange
            var Cart = new Cart
            {
                CartId = new Guid("de8b8a75-f70e-40cd-82f2-0a17394bd572"),
                Amount = (decimal)300000,
                customerId = "123",
                CartItems = new List<CartItem> { new CartItem { CartId = new Guid() } }
            };
            var context = await SeedDatabaseContext();
            var sut = new CartRepository(context);

            // Act
            sut.Add(Cart);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Cart.FirstOrDefaultAsync(x => x.CartId == Cart.CartId));
        }
        [Fact]
        public async Task DeleteCartAsync_WhenSuccessful_ShouldUpdateCart()
        {
            // Arrange
            var id = new Guid("6281f912-aa12-45af-9bfa-61472d874698");
            var context = await SeedDatabaseContext();
            var sut = new CartRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Cart>
            {
                Where = c => c.CartId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Cart.FindAsync(id));
        }
    }
}
