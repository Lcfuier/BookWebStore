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
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _data;

        public PublisherService(IUnitOfWork data)
        {
            _data = data;
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _data.Publisher.ListAllAsync(new QueryOptions<Publisher>());
        }

        public async Task<Publisher?> GetPublisherByIdAsync(Guid id)
        {
            QueryOptions<Publisher> options = new()
            {
                Includes = "Books",
                Where = p => p.PublisherId == id
            };

            return await _data.Publisher.GetAsync(options);
        }
        public async Task<Publisher?> GetPublisherByIdAsyncNoTracking(Guid? id)
        {

            return await _data.Publisher.GetByIdAsync(c => c.PublisherId.Equals(id), includeProperties: "Books");
        }
        public async Task<IEnumerable<Publisher>> GetPublishersByTermAsync(string term)
        {
            QueryOptions<Publisher> options = new()
            {
                Where = p => p.Name.Contains(term)
            };

            return await _data.Publisher.ListAllAsync(options);
        }

        public async Task AddPublisherAsync(Publisher publisher)
        {
            _data.Publisher.Add(publisher);
            await _data.SaveAsync();
        }

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            _data.Publisher.Update(publisher);
            await _data.SaveAsync();
        }

        public async Task RemovePublisherAsync(Publisher publisher)
        {
            _data.Publisher.Remove(publisher);
            await _data.SaveAsync();
        }
    }
}
