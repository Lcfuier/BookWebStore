using AutoFixture;
using AutoMapper;
using BusinessLogic.Interface;
using BussinessLogic.Interface;
using Entity.DTOs;
using Entity.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Areas.Librarian.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookWebStore.UnitTest.Controller
{
    public class CategoryControllerTests
    {
        private readonly Fixture _fixture;
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;
        private readonly CategoryController _sut;
        private readonly IMapper _mapper;
        public CategoryControllerTests()
        {
            _fixture = new Fixture();
            _logger = A.Fake<ILogger<CategoryController>>();
            _categoryService = A.Fake<ICategoryService>();
            _cacheService = A.Fake<ICacheService>();
            _sut = new CategoryController(_categoryService, _mapper);
        }
        [Fact]
        public async Task Get_WhenThereIsCacheData_ShouldReturnContactsWithStatusCode200OK()
        {
            // Arrange
            var cacheData = _fixture.CreateMany<Category>(3).ToList();
            A.CallTo(() => _cacheService.GetDataAsync<IEnumerable<Category>>("categories")).Returns(cacheData);

            // Act
            var actual = await _sut.GetAllCategories();

            // Assert
            A.CallTo(() => _cacheService.GetDataAsync<IEnumerable<Category>>("categories")).MustHaveHappenedOnceExactly();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            var result = Assert.IsAssignableFrom<IEnumerable<Category>>(actionResult.Value);
            Assert.Equal(cacheData.Count(), result.Count());
        }
    }
}
