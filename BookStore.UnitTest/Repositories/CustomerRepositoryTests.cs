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
    public class CustomerRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var Customer1 = new Customer
            {
               Id="user1",
               Email="user1@gmail.com",
               PasswordHash="Duyvip@123",
               FirstName="a",
               LastName="b",
               District="c",
               city="c",
               street="a",
               state="a",

            };
            var Customer2 = new Customer
            {
                Id = "user2",
                Email = "user2@gmail.com",
                PasswordHash = "Duyvip@123",
                FirstName = "a",
                LastName = "b",
                District = "c",
                city = "c",
                street = "a",
                state = "a",
            };
            var Customer3 = new Customer
            {
                Id = "user3",
                Email = "user3@gmail.com",
                PasswordHash = "Duyvip@123",
                FirstName = "a",
                LastName = "b",
                District = "c",
                city = "c",
                street = "a",
                state = "a",
            };
            await context.Customer.AddAsync(Customer1);
            await context.Customer.AddAsync(Customer3);
            await context.Customer.AddAsync(Customer2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetCustomersAsync_WhenSuccessful_ShouldReturnCustomers()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new CustomerRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Customer>>(actual);
            Assert.Equal(context.Customer.Count(), actual.Count());
        }
        [Fact]
        public async Task GetCustomersByIdAsync_WhenSuccessful_ShouldReturnCustomer()
        {
            // Arrange
            var id = "user1";
            var context = await SeedDatabaseContext();
            var sut = new CustomerRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Customer>
            {
                Where = c => c.Id == id
            });

            // Assert
            Assert.IsType<Customer>(actual);
        }
        [Fact]
        public async Task GetCustomersByEmailAsync_WhenSuccessful_ShouldReturnCustomer()
        {
            // Arrange
            var email = "user1@gmail.com";
            var context = await SeedDatabaseContext();
            var sut = new CustomerRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Customer>
            {
                Where = c => c.Email == email
            });

            // Assert
            Assert.IsType<Customer>(actual);
        }
        [Fact]
        public async Task AddCustomerAsync_WhenSuccessful_ShouldAddCustomer()
        {
            // Arrange
            var Customer = new Customer
            {
                Id = "user4",
                Email = "user4@gmail.com",
                PasswordHash = "Duyvip@123",
                FirstName = "a",
                LastName = "b",
                District = "c",
                city = "c",
                street = "a",
                state = "a",
            };
            var context = await SeedDatabaseContext();
            var sut = new CustomerRepository(context);

            // Act
            sut.Add(Customer);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Customer.FirstOrDefaultAsync(x => x.Id == Customer.Id));
        }
        [Fact]
        public async Task DeleteCustomerAsync_WhenSuccessful_ShouldUpdateCustomer()
        {
            // Arrange
            var id = "user1";
            var context = await SeedDatabaseContext();
            var sut = new CustomerRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Customer>
            {
                Where = c => c.Id == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Customer.FindAsync(id));
        }
    }
}
