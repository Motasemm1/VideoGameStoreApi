using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly VideoGameDbContext _context;
        public PublisherRepository(VideoGameDbContext context) => _context = context;

        public async Task AddAsync(Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
        }
        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers.Include(p => p.VideoGames).FirstOrDefaultAsync(p => p.PublisherId == id);
        }
        public async Task UpdateAsync(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Publisher?>> GetAllAsync()
        {
            return await _context.Publishers.Include(p => p.VideoGames).ThenInclude(g => g.Genre).ToListAsync();
        }


        public Task<bool> ExistsAsync(int id)
        {
           var exists =  _context.Publishers.AnyAsync(p => p.PublisherId == id);
            return exists;
        }
    }
}