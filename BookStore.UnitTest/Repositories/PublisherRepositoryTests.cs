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
    public class PublisherRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var publisher1 = new Publisher
            {
                PublisherId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                Name = "A",
            };
            var publisher2 = new Publisher
            {
                PublisherId = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25"),
                Name = "B",
            };
            var publisher3 = new Publisher
            {
                PublisherId = new Guid("eb6577bf-e5e8-46d6-bca2-fc72bca57b8f"),
                Name = "C",
            };
            await context.Publisher.AddAsync(publisher1);
            await context.Publisher.AddAsync(publisher3);
            await context.Publisher.AddAsync(publisher2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetPublishersAsync_WhenSuccessful_ShouldReturnPublishers()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new PublisherRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Publisher>>(actual);
            Assert.Equal(context.Publisher.Count(), actual.Count());
        }
        [Fact]
        public async Task GetPublishersByIdAsync_WhenSuccessful_ShouldReturnPublisher()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new PublisherRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Publisher>
            {
                Where = c => c.PublisherId == id
            });

            // Assert
            Assert.IsType<Publisher>(actual);
        }
        [Fact]
        public async Task AddPublisherAsync_WhenSuccessful_ShouldAddPublisher()
        {
            // Arrange
            var Publisher = new Publisher
            {
                PublisherId = new Guid("424f8543-b34b-4e7a-90a3-5b5271fd3224"),
                Name = "D"
            };
            var context = await SeedDatabaseContext();
            var sut = new PublisherRepository(context);

            // Act
            sut.Add(Publisher);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Publisher.FirstOrDefaultAsync(x => x.PublisherId == Publisher.PublisherId));
        }
        [Fact]
        public async Task DeletePublisherAsync_WhenSuccessful_ShouldUpdatePublisher()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new PublisherRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Publisher>
            {
                Where = c => c.PublisherId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Publisher.FindAsync(id));
        }
    }
}
