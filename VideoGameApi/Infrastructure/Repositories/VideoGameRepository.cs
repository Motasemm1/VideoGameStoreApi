using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class VideoGameRepository: IVideoGameRepository
    {
        private readonly VideoGameDbContext _context;
        public VideoGameRepository(VideoGameDbContext context) => _context = context;

        public async Task AddAsync(VideoGame game)
        {
            await _context.VideoGames.AddAsync(game);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game == null)
                return false;

            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<VideoGame>> GetAllAsync() =>
            await _context.VideoGames.Include(g => g.Genre)
                                      .Include(p => p.Publisher)
                                      .ToListAsync();

        public async Task<VideoGame?> GetByIdAsync(int id)
        {
           return  await _context.VideoGames.Include(g => g.Genre)
                                     .Include(p => p.Publisher)
                                     .FirstOrDefaultAsync(v => v.GameId == id);
        }
        public async Task UpdateAsync(VideoGame videoGame)
        {
            _context.VideoGames.Update(videoGame);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VideoGame>> SearchByNameAsync(string keyword)
        {
            return await _context.VideoGames
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .Where(g => g.Title.ToLower().Contains(keyword.ToLower()) ||
                           (g.Description != null && g.Description.ToLower().Contains(keyword.ToLower())))
                .ToListAsync();
        }
        public async Task<IQueryable<VideoGame>> GetAllWithDetailsAsQueryableAsync()
        {
            var games = _context.VideoGames
                .Include(v => v.Genre)
                .Include(v => v.Publisher)
                .AsQueryable();

            return await Task.FromResult(games);
        }

        public async Task<bool> UserVideoGameExist(Guid UserId ,int gameid)
        {
            var exists = await _context.UserVideoGames.AnyAsync(UserVideoGame => UserVideoGame.UserId == UserId && UserVideoGame.GameId == gameid);
            return exists;
        }

        public async Task AddToUserVideoGame(UserVideoGame userVideoGame)
        {
            await _context.UserVideoGames.AddAsync(userVideoGame);
            await _context.SaveChangesAsync();
        }

       
    }
}
