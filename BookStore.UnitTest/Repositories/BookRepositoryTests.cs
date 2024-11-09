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
    public class BookRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
           /* var publisher1 = new Publisher
            {
                PublisherId = new Guid("206afff8-7306-455a-8f24-8bcc26f25698"),
                Name = "A",
            };
            var publisher2 = new Publisher
            {
                PublisherId = new Guid("3e3d1c3c-3433-4148-9ba4-e31e02437ebf"),
                Name = "B",
            };
          
            await context.Publisher.AddAsync(publisher1);
            await context.Publisher.AddAsync(publisher2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            var author1 = new Author
            {
                AuthorId = new Guid("e005ab54-17e9-42d6-932e-948e43904bc6"),
                FirstName = "A",
                LastName = "Nguyen",
                PhoneNumber = "123456890",
            };
            var author2 = new Author
            {
                AuthorId = new Guid("a17f5486-92ec-485c-aeb2-7191a2ad61e2"),
                FirstName = "B",
                LastName = "Tran",
                PhoneNumber = "123456790",
            };
          
            await context.Author.AddAsync(author1);
            await context.Author.AddAsync(author2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();*/
          
            var book1 = new Book
            {
                BookId = new Guid("7cdc7ae9-d8e1-47b1-b195-ab5a7a96a774"),
                Title = "Test1",
                Description = "ceate book1",
                Isbn13 = "123312312",
                Inventory = 3,
                Price = 300000,
                DiscountPercent = (Decimal)0.1,
                NumberOfPage = 200,
                PublicationDate = DateTime.Now,
                ImageURL = "af2b3fba-fe9c-4ad1-900c-d9e02ff6d195.jpg",
                LastModifiedDate = DateTime.Now,
                authorId = new Guid("e005ab54-17e9-42d6-932e-948e43904bc6"),
                publisherID = new Guid("206afff8-7306-455a-8f24-8bcc26f25698")
            };
            var book2 = new Book
            {
                BookId = new Guid("4cb8481a-fe4e-435a-9d34-aa6a64126b90"),
                Title = "Test2",
                Description = "ceate book2",
                Isbn13 = "123312312",
                Inventory = 3,
                Price = 500000,
                DiscountPercent = (Decimal)0.2,
                NumberOfPage = 200,
                PublicationDate = DateTime.Now,
                ImageURL = "af2b3fba-fe9c-4ad1-900c-d9e02ff6d195.jpg",
                LastModifiedDate = DateTime.Now,
                authorId = new Guid("a17f5486-92ec-485c-aeb2-7191a2ad61e2"),
                publisherID = new Guid("3e3d1c3c-3433-4148-9ba4-e31e02437ebf")
            };
            await context.Book.AddAsync(book1);
            await context.Book.AddAsync(book2);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            /* var BookCategory1 = new BookCategory
             {
                 CategoryId = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25"),
                 BookId = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25")
             };
             var BookCategory2 = new BookCategory
             {
                 CategoryId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                 BookId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0")
             };
             await context.Category.AddAsync(book1);
             await context.Book.AddAsync(book2);
             await context.SaveChangesAsync();*/
            //context.ChangeTracker.Clear();
           /* var category1 = new Category
            {
                CategoryId = new Guid("4a73cfa8-80ca-4222-8322-25e50dd922a1"),
                Name = "Action",
                Books=new List<Book> { book1, book2 }
            };
            var category2 = new Category
            {
                CategoryId = new Guid("dd113f21-38eb-4c3b-a3bd-75fd53bd830b"),
                Name = "Comic",
                Books = new List<Book> { book1}
            };
            var category3 = new Category
            {
                CategoryId = new Guid("64e307dc-9f46-49ce-ad60-a320d823e560"),
                Name = "Comedy",
                Books = new List<Book> { book2 }
            };
            await context.Category.AddAsync(category1);
            await context.Category.AddAsync(category2);
            await context.Category.AddAsync(category3);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();*/
            return context;
        }
        [Fact]
        public async Task GetBooksAsync_WhenSuccessful_ShouldReturnBooks()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new BookRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Book>>(actual);
            Assert.Equal(context.Book.Count(), actual.Count());
        }
        [Fact]
        public async Task GetBooksByIdAsync_WhenSuccessful_ShouldReturnBook()
        {
            // Arrange
            var id = new Guid("7cdc7ae9-d8e1-47b1-b195-ab5a7a96a774");
            var context = await SeedDatabaseContext();
            var sut = new BookRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Book>
            {
                Where = c => c.BookId == id
            });

            // Assert
            Assert.IsType<Book>(actual);
        }
        [Fact]
        public async Task AddBookAsync_WhenSuccessful_ShouldAddBook()
        {
            // Arrange
            var Book = new Book
            {
                BookId = new Guid("424f8543-b34b-4e7a-90a3-5b5271fd3224"),
                Title = "Test3",
                Description = "ceate book3",
                Isbn13 = "123312312",
                Inventory = 3,
                Price = 500000,
                DiscountPercent = (Decimal)0.2,
                NumberOfPage = 200,
                PublicationDate = DateTime.Now,
                ImageURL = "af2b3fba-fe9c-4ad1-900c-d9e02ff6d195.jpg",
                LastModifiedDate = DateTime.Now,
                authorId = new Guid("eb6577bf-e5e8-46d6-bca2-fc72bca57b8f"),
                publisherID = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25")
            };
            var context = await SeedDatabaseContext();
            var sut = new BookRepository(context);

            // Act
            sut.Add(Book);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Book.FirstOrDefaultAsync(x => x.BookId == Book.BookId));
        }
        [Fact]
        public async Task DeleteBookAsync_WhenSuccessful_ShouldUpdateBook()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new BookRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Book>
            {
                Where = c => c.BookId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Book.FindAsync(id));
        }
    }
}
