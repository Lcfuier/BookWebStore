using BussinessLogic.Interface;
using DataAccess.Interface;
using Entity.Models;
using Entity.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _data;
        public AuthorService(IUnitOfWork data)
        {
            _data = data;
        }
        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _data.Author.ListAllAsync(new QueryOptions<Author>());
        }

        public async Task<Author?> GetAuthorByIdAsync(Guid? id)
        {
            QueryOptions<Author> options = new()
            {
                Where = a => a.AuthorId == id,
                Includes = "Books"
            };

            return await _data.Author.GetAsync(options);
        }
        public async Task<Author?> GetAuthorByIdAsyncNoTracking(Guid? id)
        {

            return await _data.Author.GetByIdAsync(c=>c.AuthorId.Equals(id),includeProperties:"Books");
        }

        public async Task<IEnumerable<Author>> GetAuthorsByTermAsync(string term)
        {
            QueryOptions<Author> options = new()
            {
                Where = a => a.FirstName.Contains(term) || a.LastName.Contains(term)
            };

            return await _data.Author.ListAllAsync(options);
        }

        public async Task AddAuthorAsync(Author author)
        {
            _data.Author.Add(author);
            await _data.SaveAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _data.Author.Update(author);
            await _data.SaveAsync();
        }

        public async Task RemoveAuthorAsync(Author author)
        {
            _data.Author.Remove(author);
            await _data.SaveAsync();
        }
    }
}
