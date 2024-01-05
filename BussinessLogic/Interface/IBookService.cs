using Entity.DTOs;
using Entity.Models;
using Entity.Query;

namespace BussinessLogic.Interface
{
    public interface IBookService
    {
        Task UpdateBookAsync(BookDTO bookDto);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetAllBooksAsyncNoTracking();
        Task<IEnumerable<Book>> GetBooksByTerm(string term);
        Task RemoveBookAsync(Book bookDto);
        Task UpdateBookAsync(Book book);
        Task<IEnumerable<Book>> GetBooksByAuthorId(Guid authorId);
        Task<IEnumerable<Book>> GetBooksByCategory(Guid authorId);
        Task AddBookAsync(Book book);
        Task AddBookAsync(BookDTO bookDto);
        Task<BookDTO?> GetBookDtoByIdAsync(Guid? id);
        Task<IEnumerable<Book>> GetBooksByTermAsync(string? term );
        Task<Book> GetBookByIdAsync(Guid? id);
        Task<int> GetBookCountAsync();
    }
}