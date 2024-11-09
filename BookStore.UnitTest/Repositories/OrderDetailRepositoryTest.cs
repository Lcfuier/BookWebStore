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
    public class OrderDetailRepositoryTest
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var OrderDetail1 = new OrderDetail
            {
                OrderDetailId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                OrderId = new Guid("863a138c-4130-4fed-8c27-c4b65995af14"),
                BookId = new Guid("524317a8-3f34-4cba-b8ec-13c9e74cd2dc"),
                Price = (decimal)100000,
                Quantity = 2
            };
            var OrderDetail2 = new OrderDetail
            {
                OrderDetailId = new Guid("d5c24f63-e7d7-4839-96e0-a3353e111c32"),
                OrderId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            var OrderDetail3 = new OrderDetail
            {
                OrderDetailId = new Guid("407e08a7-e0bd-4274-b0f9-40877e6e290c"),
                OrderId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            await context.OrderDetail.AddAsync(OrderDetail1);
            await context.OrderDetail.AddAsync(OrderDetail3);
            await context.OrderDetail.AddAsync(OrderDetail2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetOrderDetailsAsync_WhenSuccessful_ShouldReturnOrderDetails()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new OrderDetailRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<OrderDetail>>(actual);
            Assert.Equal(context.OrderDetail.Count(), actual.Count());
        }
        [Fact]
        public async Task GetOrderDetailsByIdAsync_WhenSuccessful_ShouldReturnOrderDetail()
        {
            // Arrange
            var id = new Guid("d5c24f63-e7d7-4839-96e0-a3353e111c32");
            var context = await SeedDatabaseContext();
            var sut = new OrderDetailRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<OrderDetail>
            {
                Where = c => c.OrderDetailId == id
            });

            // Assert
            Assert.IsType<OrderDetail>(actual);
        }
        [Fact]
        public async Task AddOrderDetailAsync_WhenSuccessful_ShouldAddOrderDetail()
        {
            // Arrange
            var OrderDetail = new OrderDetail
            {
                OrderDetailId = new Guid("7a1c3694-2cc8-4e1b-a0f3-65c32d823334"),
                OrderId = new Guid(),
                BookId = new Guid(),
                Price = (decimal)200000,
                Quantity = 2
            };
            var context = await SeedDatabaseContext();
            var sut = new OrderDetailRepository(context);

            // Act
            sut.Add(OrderDetail);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.OrderDetail.FirstOrDefaultAsync(x => x.OrderDetailId == OrderDetail.OrderDetailId));
        }
        [Fact]
        public async Task DeleteOrderDetailAsync_WhenSuccessful_ShouldUpdateOrderDetail()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new OrderDetailRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<OrderDetail>
            {
                Where = c => c.OrderDetailId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.OrderDetail.FindAsync(id));
        }
    }
}
