using AutoFixture;
using AutoMapper;
using BookWebStore.UnitTest.Mocks;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using DataAccess.Interface;
using DataAccess.UnitOfWork;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookWebStore.UnitTest.Services
{
    public class CategoryServiceTest
    {
        private readonly Fixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _sut;
        public CategoryServiceTest()
        {
            _fixture = new Fixture();
            var context = MockDbContext.CreateMockDbContext();
            _unitOfWork = new UnitOfWork(context);
            _mapper = A.Fake<IMapper>();
            _categoryRepository = A.Fake<ICategoryRepository>();
            _sut = new CategoryService(_unitOfWork);
        }
        [Fact]
        public async Task GetCategoriesAsync_WhenSuccessful_ShouldReturnCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<Category>(3).ToList();
            A.CallTo(() => _categoryRepository.ListAllAsync(new QueryOptions<Category>())).Returns(categories);

            // Act
            var actual = await _sut.GetAllCategoryAsync();

            // Assert
            A.CallTo(() => _categoryRepository.ListAllAsync(new QueryOptions<Category>())).MustHaveHappenedOnceExactly();
            Assert.IsAssignableFrom<IEnumerable<Category>>(actual);
            Assert.Equal(categories.Count(), actual.Count());
        }
        [Fact]
        public async Task AddCategoryAsync_WhenSuccessful_ShouldAddAndReturnCategory()
        {
            // Arrange
            var category = _fixture.Create<Category>();
            var response = _fixture.Create<CategoryDTO>();
            A.CallTo(() => _mapper.Map<Category>(A<CategoryDTO>._)).Returns(category);
            A.CallTo(() => _mapper.Map<CategoryDTO>(A<Category>._)).Returns(response);

            // Act
            var actual =  _sut.AddCategoryAsync(category);

            // Assert
            A.CallTo(() => _categoryRepository.Add(A<Category>._)).MustHaveHappenedOnceExactly();
            Assert.IsAssignableFrom<CategoryDTO>(actual);
        }
        [Fact]
        public async Task DeleteCategoryAsync_WhenSuccessful_ShouldDeleteCategory()
        {
            // Arrange
            var obj = _fixture.Create<Category>();

            // Act
            await _sut.RemoveCategoryAsync(obj);

            // Assert
            A.CallTo(() => _categoryRepository.Remove(A<Category>._)).MustHaveHappenedOnceExactly();
        }
    }
}
