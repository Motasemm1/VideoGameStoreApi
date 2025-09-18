using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VideoGameDbContext _context;
        public UserRepository(VideoGameDbContext context) => _context = context;

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            var user = await _context.Users
        .Include(u => u.UserVideoGames)
            .ThenInclude(uv => uv.VideoGame)
                .ThenInclude(g => g.Genre)
        .Include(u => u.UserVideoGames)
            .ThenInclude(uv => uv.VideoGame)
                .ThenInclude(g => g.Publisher)
        .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.VideoGame)
        .FirstOrDefaultAsync(u => u.UserId == userId);
            return user!;

        }
        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.Include(u => u.Orders)
                              .ThenInclude(o => o.OrderItems)
                              .ToListAsync();
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task Savechanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByIdWithOrdersAndGamesAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                .Include(u => u.UserVideoGames)
                    .ThenInclude(uv => uv.VideoGame)
                        .ThenInclude(g => g.Genre)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }
    }

}

