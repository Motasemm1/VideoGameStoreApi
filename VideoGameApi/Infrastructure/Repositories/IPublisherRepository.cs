using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Repositories
{
    public interface IPublisherRepository
    {
        Task<Publisher?> GetByIdAsync(int id);
        Task<IEnumerable<Publisher?>> GetAllAsync();
        Task AddAsync(Publisher publisher);
        Task UpdateAsync(Publisher publisher);
        Task DeleteAsync(Publisher publisher);
        Task<bool> ExistsAsync(int id);
    }
}
