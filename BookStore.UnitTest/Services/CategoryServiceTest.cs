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
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

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
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var context = MockDbContext.CreateMockDbContext();
            _categoryRepository = A.Fake<ICategoryRepository>();

            // Setup the UnitOfWork to return the mocked repository
            _unitOfWork = A.Fake<IUnitOfWork>();
            A.CallTo(() => _unitOfWork.Category).Returns(_categoryRepository);

            _mapper = A.Fake<IMapper>();
            _sut = new CategoryService(_unitOfWork);
        }

        [Fact]
        public async Task GetCategoriesAsync_WhenSuccessful_ShouldReturnCategories()
        {
            string include = null;
            Expression<Func<Category, bool>>? filter = null;
            bool tracked = false;
            // Arrange
            var categories = _fixture.CreateMany<Category>(3).ToList();
            A.CallTo(() => _categoryRepository.GetAll(filter,include,tracked)).Returns(categories);

            // Act
            var actual = await _sut.GetAllCategoryAsync();

            // Assert
            A.CallTo(() => _categoryRepository.GetAll(filter, include, tracked)).MustHaveHappenedOnceExactly();
            actual.Should().BeAssignableTo<IEnumerable<Category>>().And.HaveCount(categories.Count).And.BeEquivalentTo(categories);
        }

        [Fact]
        public async Task AddCategoryAsync_WhenSuccessful_ShouldAddAndReturnCategory()
        {
            // Arrange
            var category = _fixture.Create<Category>();

            // Configure the mock to return the expected DTO when mapped

            // Act
            await _sut.AddCategoryAsync(category);  // Note the 'await' here

            // Assert
            A.CallTo(() => _categoryRepository.Add(A<Category>.Ignored)).MustHaveHappenedOnceExactly();  // Ensure Add was called once
            A.CallTo(() => _unitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
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
