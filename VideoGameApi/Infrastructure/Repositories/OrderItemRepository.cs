using Microsoft.EntityFrameworkCore;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly VideoGameDbContext _context;

        public OrderItemRepository(VideoGameDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> GetByIdAsync(int id) =>
            await _context.OrderItems.Include(oi => oi.VideoGame).FirstOrDefaultAsync(oi => oi.Id == id);

        public async Task<IEnumerable<OrderItem>> GetAllAsync() =>
            await _context.OrderItems.Include(oi => oi.VideoGame).ToListAsync();

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId) =>
            await _context.OrderItems.Where(oi => oi.OrderId == orderId)
                                     .Include(oi => oi.VideoGame)
                                     .ToListAsync();
    }

}

