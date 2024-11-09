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
    public class AuthorRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var author1 = new Author
            {
                AuthorId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                FirstName = "A",
                LastName    = "Nguyen",
                PhoneNumber= "123456890",
            };
            var author2  = new Author
            {
                AuthorId = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25"),
                FirstName = "B",
                LastName="Tran",
                PhoneNumber = "123456790",
            };
            var author3 = new Author
            {
                AuthorId = new Guid("eb6577bf-e5e8-46d6-bca2-fc72bca57b8f"),
                FirstName = "C",
                LastName = "Pham",
                PhoneNumber = "123456890",
            };
            await context.Author.AddAsync(author1);
            await context.Author.AddAsync(author2);
            await context.Author.AddAsync(author3);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetAuthorsAsync_WhenSuccessful_ShouldReturnAuthors()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new AuthorRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Author>>(actual);
            Assert.Equal(context.Author.Count(), actual.Count());
        }
        [Fact]
        public async Task GetAuthorByIdAsync_WhenSuccessful_ShouldReturnAuthor()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new AuthorRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Author>
            {
                Where = c => c.AuthorId == id
            });

            // Assert
            Assert.IsType<Author>(actual);
        }
        [Fact]
        public async Task AddAuthorAsync_WhenSuccessful_ShouldAddAuthor()
        {
            // Arrange
            var author = new Author
            {
                AuthorId = new Guid("424f8543-b34b-4e7a-90a3-5b5271fd3224"),
                FirstName = "D",
                LastName="Tran",
                PhoneNumber = "1234567890",
            };
            var context = await SeedDatabaseContext();
            var sut = new AuthorRepository(context);

            // Act
            sut.Add(author);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Author.FirstOrDefaultAsync(x => x.AuthorId == author.AuthorId));
        }
        [Fact]
        public async Task DeleteAuthorAsync_WhenSuccessful_ShouldUpdateAuthor()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new AuthorRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Author>
            {
                Includes = "Books",
                Where = c => c.AuthorId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Author.FindAsync(id));
        }
    }
}
