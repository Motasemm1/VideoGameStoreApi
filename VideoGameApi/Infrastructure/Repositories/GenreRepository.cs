using Microsoft.EntityFrameworkCore;
using VideoGameApi.Application.DTO.Genre;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly VideoGameDbContext _context;
        public GenreRepository(VideoGameDbContext context) => _context = context;

        public async Task AddAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Genre genre)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Genres.AnyAsync(g => g.GenreId == id);
        }

        public async Task<IEnumerable<Genre?>> GetAllAsync()
        {
            return await _context.Genres.Include(g => g.VideoGames).ThenInclude(p => p.Publisher).ToListAsync();
                
        }
        public async Task<Genre?> GetByIdAsync(int id) =>
            await _context.Genres.Include(g => g.VideoGames).ThenInclude(p => p.Publisher).FirstOrDefaultAsync(g => g.GenreId == id);

        public async Task UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}
