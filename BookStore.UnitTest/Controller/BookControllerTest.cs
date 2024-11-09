using AutoFixture;
using BussinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Interface;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace BookWebStore.UnitTest.Controllers
{
    public class BookControllerTest
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookWebStoreDbContext _context;
        private readonly BookController _controller;
        private readonly Fixture _fixture;

        public BookControllerTest()
        {
            _fixture = new Fixture();
            _bookService = A.Fake<IBookService>();
            _categoryService = A.Fake<ICategoryService>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _context = A.Fake<BookWebStoreDbContext>();

            _controller = new BookController(_bookService, _categoryService, _unitOfWork, _context);
        }

        [Fact]
        public async Task Detail_WhenBookIsNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            A.CallTo(() => _bookService.GetBookByIdAsync(bookId)).Returns(Task.FromResult<Book?>(null));

            // Act
            var result = await _controller.Detail(bookId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

       
        [Fact]
        public async Task GetBookByAuthor_WhenCalled_ShouldReturnViewWithBooks()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var books = _fixture.CreateMany<Book>(3).AsEnumerable();
            A.CallTo(() => _bookService.GetBooksByAuthorId(authorId)).Returns(Task.FromResult(books));

            // Act
            var result = await _controller.GetBookByAuthor(authorId);

            // Assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetBookByCategory_WhenCalled_ShouldReturnViewWithCategoryAndBooks()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = _fixture.Create<Category>();
            var books = _fixture.CreateMany<Book>(3).AsEnumerable();

            A.CallTo(() => _categoryService.GetCategoryByIdAsync(categoryId)).Returns(Task.FromResult(category));
            A.CallTo(() => _bookService.GetBooksByCategory(categoryId)).Returns(Task.FromResult(books));

            // Act
            var result = await _controller.GetBookByCategory(categoryId);

            // Assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult?.Model as BookCategoryDTO;
            model.Should().NotBeNull();
            model!.category.Should().BeEquivalentTo(category);
            model.books.Should().BeEquivalentTo(books);
        }
    }
}
