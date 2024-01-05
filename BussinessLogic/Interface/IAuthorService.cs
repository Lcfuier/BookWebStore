using Entity.Models;

namespace BussinessLogic.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(Guid? id);
        Task<IEnumerable<Author>> GetAuthorsByTermAsync(string term);
        Task<Author?> GetAuthorByIdAsyncNoTracking(Guid? id);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task RemoveAuthorAsync(Author author);
    }
}