using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        private readonly VideoGameDbContext _context;
        public OrderRepository(VideoGameDbContext context) => _context = context;

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId) =>
            await _context.Orders
                          .Where(o => o.UserId == userId)
                          .Include(o => o.OrderItems)
                          .ThenInclude(oi => oi.VideoGame)
                          .ToListAsync();

        public async Task<Order?> GetByIdAsync(int id) =>
            await _context.Orders.Include(u => u.User).Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.VideoGame)
                                 .FirstOrDefaultAsync(o => o.Id == id);

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
