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
    public class OrderRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var Order1 = new Order
            {
                OrderId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                OrderDate = DateTime.Now,
                ShippingDate= DateTime.Now.AddDays(3),
                Name="a",
                PhoneNumber="123456788",
                TotalAmount=(decimal)300000,
                TrackingNumber="123455",
                PaymentIntentId="124566",
                SessionId="acbett",
                Carrier ="fghh12",
                customerId="123",
                PaymentStatus="Success",
                District="HCC",
                Ward="Tb",
                Address="124"
            };
            var Order2 = new Order
            {
                OrderId = new Guid("d5c24f63-e7d7-4839-96e0-a3353e111c32"),
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                Name = "a",
                PhoneNumber = "123456788",
                TotalAmount = (decimal)300000,
                TrackingNumber = "123455",
                PaymentIntentId = "124566",
                SessionId = "acbett",
                Carrier = "fghh12",
                customerId = "123",
                PaymentStatus = "Success",
                District = "HCC",
                Ward = "Tb",
                Address = "124"
            };
            var Order3 = new Order
            {
                OrderId = new Guid("407e08a7-e0bd-4274-b0f9-40877e6e290c"),
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                Name = "a",
                PhoneNumber = "123456788",
                TotalAmount = (decimal)300000,
                TrackingNumber = "123455",
                PaymentIntentId = "124566",
                SessionId = "acbett",
                Carrier = "fghh12",
                customerId = "123",
                PaymentStatus = "Success",
                District = "HCC",
                Ward = "Tb",
                Address = "124"
            };
            await context.Order.AddAsync(Order1);
            await context.Order.AddAsync(Order3);
            await context.Order.AddAsync(Order2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetOrdersAsync_WhenSuccessful_ShouldReturnOrders()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new OrderRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Order>>(actual);
            Assert.Equal(context.Order.Count(), actual.Count());
        }
        [Fact]
        public async Task GetOrdersByIdAsync_WhenSuccessful_ShouldReturnOrder()
        {
            // Arrange
            var id = new Guid("407e08a7-e0bd-4274-b0f9-40877e6e290c");
            var context = await SeedDatabaseContext();
            var sut = new OrderRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Order>
            {
                Where = c => c.OrderId == id
            });

            // Assert
            Assert.IsType<Order>(actual);
        }
        [Fact]
        public async Task AddOrderAsync_WhenSuccessful_ShouldAddOrder()
        {
            // Arrange
            var Order = new Order
            {
                OrderId = new Guid("7a1c3694-2cc8-4e1b-a0f3-65c32d823334"),
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                Name = "a",
                PhoneNumber = "123456788",
                TotalAmount = (decimal)300000,
                TrackingNumber = "123455",
                PaymentIntentId = "124566",
                SessionId = "acbett",
                Carrier = "fghh12",
                customerId = "123",
                PaymentStatus = "Success",
                District = "HCC",
                Ward = "Tb",
                Address = "124"
            };
            var context = await SeedDatabaseContext();
            var sut = new OrderRepository(context);

            // Act
            sut.Add(Order);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Order.FirstOrDefaultAsync(x => x.OrderId == Order.OrderId));
        }
        [Fact]
        public async Task DeleteOrderAsync_WhenSuccessful_ShouldUpdateOrder()
        {
            // Arrange
            var id = new Guid("407e08a7-e0bd-4274-b0f9-40877e6e290c");
            var context = await SeedDatabaseContext();
            var sut = new OrderRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Order>
            {
                Where = c => c.OrderId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Order.FindAsync(id));
        }
    }
}
