using Entity.Models;

namespace BussinessLogic.Interface
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(Guid id);
        Task<IEnumerable<Publisher>> GetPublishersByTermAsync(string term);
        Task<Publisher?> GetPublisherByIdAsyncNoTracking(Guid? id);
        Task AddPublisherAsync(Publisher category);
        Task UpdatePublisherAsync(Publisher category);
        Task RemovePublisherAsync(Publisher category);
    }
}