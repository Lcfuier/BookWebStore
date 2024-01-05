using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BussinessLogic.Interface;
using DataAccess.Interface;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using Microsoft.Extensions.Options;

namespace BussinessLogic.Service
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _data;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            QueryOptions<Book> options = new()
            {
                Includes = "Author, Categories, Publisher"
            };

            return await _data.Book.ListAllAsync(options);
        }
        public async Task<BookDTO?> GetBookDtoByIdAsync(Guid? id)
        {
            QueryOptions<Book> options = new()
            {
                Where = b => b.BookId == id,
                Includes = "Author, Publisher, Categories"
            };

            Book? book = await _data.Book.GetAsync(options);
            if (book is null)
            {
                return null;
            }

            return _mapper.Map<BookDTO>(book);
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsyncNoTracking()
        {
            var book = await _data.Book.GetAll(includeProperties: "Author,Categories");

            return book;
        }
        public async Task<IEnumerable<Book>> GetBooksByAuthorId(Guid authorId)
        {
            QueryOptions<Book> options = new()
            {
                Where = b => b.authorId.Equals(authorId),
                Includes = "Author, Publisher, Categories"
            };
            var book = await _data.Book.ListAllAsync(options);

            return book;
        }
        public async Task<IEnumerable<Book>> GetBooksByCategory(Guid CategoryId)
        {
            QueryOptions<Book> options = new()
            {
                Where = b => b.Categories.Any(c => c.CategoryId.Equals(CategoryId)),
                Includes = "Author, Publisher, Categories"
            };
            var book = await _data.Book.ListAllAsync(options);

            return book;
        }
        public async Task<IEnumerable<Book>> GetBooksByTermAsync(string? term)
        {
            if(!string.IsNullOrEmpty(term)) { 
            }
            var book = await _data.Book.GetAll(s=>s.Title.Contains(term),includeProperties: "Author");

            return book;
        }
        public async Task<IEnumerable<Book>> GetBooksByTerm(string term)
        {
            QueryOptions<Book> options = new()
            {
                Where = b => b.Title.Contains(term)
            };

            return await _data.Book.ListAllAsync(options);
        }

        public async Task<Book> GetBookByIdAsync(Guid? id)
        {
            return await _data.Book.GetByIdAsync(r => r.BookId.Equals(id), includeProperties: "Author,Categories,Publisher"); 
        }
        public async Task<int> GetBookCountAsync()
        {
            return await _data.Book.CountAsync();
        }
        public async Task RemoveBookAsync(Book bookDto)
        {
            _data.Book.Remove(bookDto);
            await _data.SaveAsync();
        }
        public async Task UpdateBookAsync(Book book)
        {
            _data.Book.Update(book);
            await _data.SaveAsync();
        }
        public async Task UpdateBookAsync(BookDTO bookDto)
        {
            QueryOptions<Book> options = new()
            {
                Where = b => b.BookId == bookDto.BookId,
                Includes = "Categories"
            };

            Book bookFromDb = await _data.Book.GetAsync(options);
            bookFromDb.Title = bookDto.Title;
            bookFromDb.Description = bookDto.Description;
            bookFromDb.Isbn13 = bookDto.Isbn13;
            bookFromDb.Inventory = bookDto.Inventory;
            bookFromDb.Price = bookDto.Price;
            bookFromDb.DiscountPercent = bookDto.DiscountPercent;
            bookFromDb.NumberOfPage = bookDto.NumberOfPage;
            bookFromDb.PublicationDate = bookDto.PublicationDate;
            bookFromDb.ImageURL = bookDto.ImageUrl;
            bookFromDb.authorId = bookDto.AuthorId;
            bookFromDb.publisherID = bookDto.PublisherId;

            await _data.Book.AddNewCategoryAsync(bookFromDb, bookDto.CategoryIds, _data.Category);

            // don't need to call _data.Books.Update(bookDto) - db context is tracking changes 
            // because retrieved bookDto with categories from db at the beginning
            _data.Book.Update(bookFromDb);
            await _data.SaveAsync();
        }
        public async Task AddBookAsync(Book book)
        {
            _data.Book.Add(book);
            await _data.SaveAsync();
        }
        public async Task AddBookAsync(BookDTO bookDto)
        {
            Book book = _mapper.Map<Book>(bookDto);

            await _data.Book.AddNewCategoryAsync(book, bookDto.CategoryIds, _data.Category);

            _data.Book.Add(book);
            await _data.SaveAsync();
        }
    }
}
    
