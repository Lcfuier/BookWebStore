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
    public class CartItemRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var CartItem1 = new CartItem
            {
                CartItemID = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                CartId= new Guid("863a138c-4130-4fed-8c27-c4b65995af14"),
                BookId= new Guid("524317a8-3f34-4cba-b8ec-13c9e74cd2dc"),
                Price=(decimal)100000,
                Quantity=2
            };
            var CartItem2 = new CartItem
            {
                CartItemID = new Guid("d5c24f63-e7d7-4839-96e0-a3353e111c32"),
                CartId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            var CartItem3 = new CartItem
            {
                CartItemID = new Guid("407e08a7-e0bd-4274-b0f9-40877e6e290c"),
                CartId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            await context.CartItem.AddAsync(CartItem1);
            await context.CartItem.AddAsync(CartItem3);
            await context.CartItem.AddAsync(CartItem2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetCartItemsAsync_WhenSuccessful_ShouldReturnCartItems()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new CartItemRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<CartItem>>(actual);
            Assert.Equal(context.CartItem.Count(), actual.Count());
        }
        [Fact]
        public async Task GetCartItemsByIdAsync_WhenSuccessful_ShouldReturnCartItem()
        {
            // Arrange
            var id = new Guid("d5c24f63-e7d7-4839-96e0-a3353e111c32");
            var context = await SeedDatabaseContext();
            var sut = new CartItemRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<CartItem>
            {
                Where = c => c.CartItemID == id
            });

            // Assert
            Assert.IsType<CartItem>(actual);
        }
        [Fact]
        public async Task AddCartItemAsync_WhenSuccessful_ShouldAddCartItem()
        {
            // Arrange
            var CartItem = new CartItem
            {
                CartItemID = new Guid("7a1c3694-2cc8-4e1b-a0f3-65c32d823334"),
                CartId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            var context = await SeedDatabaseContext();
            var sut = new CartItemRepository(context);

            // Act
            sut.Add(CartItem);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.CartItem.FirstOrDefaultAsync(x => x.CartItemID == CartItem.CartItemID));
        }
        [Fact]
        public async Task DeleteCartItemAsync_WhenSuccessful_ShouldUpdateCartItem()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new CartItemRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<CartItem>
            {
                Where = c => c.CartItemID == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.CartItem.FindAsync(id));
        }
    }
}
