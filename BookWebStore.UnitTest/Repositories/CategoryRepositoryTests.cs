using BookWebStore.UnitTest.Mocks;
using Castle.Components.DictionaryAdapter.Xml;
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
using Xunit;
using Assert = Xunit.Assert;

namespace BookWebStore.UnitTest.Repositories
{
    public class CategoryRepositoryTests
    {
        private async Task<BookWebStoreDbContext> SeedDatabaseContext()
        {
            var context = MockDbContext.CreateMockDbContext();
            var category1 = new Category
            {
                CategoryId = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0"),
                Name = "Action",
            };
            var category2 = new Category
            {
                CategoryId = new Guid("ae480964-1458-4de2-90d5-c08ef090fb25"),
                Name = "Comic",
            };
            var category3 = new Category
            {
                CategoryId = new Guid("eb6577bf-e5e8-46d6-bca2-fc72bca57b8f"),
                Name = "Comedy",
            };
            await context.Category.AddAsync(category1);
            await context.Category.AddAsync(category2);
            await context.Category.AddAsync(category3);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return context;
        }
        [Fact]
        public async Task GetCategoriesAsync_WhenSuccessful_ShouldReturnCategories()
        {
            // Arrange
            var context = await SeedDatabaseContext();
            var sut = new CategoryRepository(context);

            // Act
            var actual = await sut.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Category>>(actual);
            Assert.Equal(context.Category.Count(), actual.Count());
        }
        [Fact]
        public async Task GetCategoriesByIdAsync_WhenSuccessful_ShouldReturnCategory()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new CategoryRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Category>
            {
                Includes = "Books",
                Where = c => c.CategoryId == id
            });

            // Assert
            Assert.IsType<Category>(actual);
        }
        [Fact]
        public async Task AddCategoryAsync_WhenSuccessful_ShouldAddCategory()
        {
            // Arrange
            var category = new Category
            {
               CategoryId= new Guid("424f8543-b34b-4e7a-90a3-5b5271fd3224"),
               Name="Art History"
            };
            var context = await SeedDatabaseContext();
            var sut = new CategoryRepository(context);

            // Act
            sut.Add(category);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(await context.Category.FirstOrDefaultAsync(x => x.CategoryId == category.CategoryId));
        }
        [Fact]
        public async Task DeleteCategoryAsync_WhenSuccessful_ShouldUpdateCategory()
        {
            // Arrange
            var id = new Guid("cf7dd825-4ae5-4cb9-b399-e48fffcfc2c0");
            var context = await SeedDatabaseContext();
            var sut = new CategoryRepository(context);

            // Act
            var actual = await sut.GetAsync(new QueryOptions<Category>
            {
                Includes = "Books",
                Where = c => c.CategoryId == id
            });
            if (actual != null)
            {
                sut.Remove(actual);
            }
            await context.SaveChangesAsync();

            // Assert
            Assert.Null(await context.Category.FindAsync(id));
        }
    }
}
