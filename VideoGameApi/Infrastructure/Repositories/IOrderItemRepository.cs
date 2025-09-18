using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    }
}
