using AutoMapper;
using BusinessLogic.Interface;
using BussinessLogic.Interface;
using Entity.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Presentation.Areas.Librarian.Controllers;
using Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWebStore.UnitTest.Controller
{
    public class HomeControllerTest
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;
        private readonly HomeController _sut;
        public HomeControllerTest()
        {
            _logger = A.Fake<ILogger<HomeController>>();
            _categoryService = A.Fake<ICategoryService>();
            _memoryCache = A.Fake<IMemoryCache>();
            _bookService=A.Fake<IBookService>();
            _sut = new HomeController(_logger,_bookService,_categoryService,_memoryCache);
        }
        [Fact]
        public async Task HomeController_Index_ReturnSuccess()
        {
            // Arrange
            var Books = A.Fake<IEnumerable<Book>>();
            A.CallTo(() => _bookService.GetAllBooksAsync()).Returns(Books);
            // Act
            var actual = await _sut.Index(Guid.Empty,"",page:1);

            // Assert
            actual.Should().BeOfType<ViewResult>();
            actual.Should().NotBeNull();
        }
    }
}
