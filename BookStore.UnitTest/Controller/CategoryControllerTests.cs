using AutoFixture;
using AutoMapper;
using BusinessLogic.Interface;
using BussinessLogic.Interface;
using Entity.DTOs;
using Entity.Models;
using FakeItEasy;
using FluentAssertions;
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
            _mapper=A.Fake<IMapper>();  
            _sut = new CategoryController(_categoryService, _mapper);
        }
        [Fact]
        public async Task Get_WhenThereAreContacts_ShouldReturnContactsWithStatusCode200OK()
        {
            // Arrange
            var Categorys = A.Fake<IEnumerable<Category>>();
            A.CallTo(() => _categoryService.GetAllCategoryAsync()).Returns(Categorys);
            // Act
            var actual = await _sut.GetAllCategories();

            // Assert
            actual.Should().BeOfType<JsonResult>();
            
        }
    }
}
