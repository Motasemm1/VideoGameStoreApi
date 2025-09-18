using VideoGameApi.Application.DTO.Orders;
using VideoGameApi.Domain;

namespace VideoGameApi.Application.Intefaces
{
    public interface IOrderService
    {
        Task<orderDto> CreateOrderAsync(Guid userId, CreateOrderDto createOrderDto);
        Task <UserOrdersResponseDto> GetUserOrders(Guid id);
        Task<ICollection<UserSummaryDto>> GetAllOrders();
        Task<OrderDetailsDto> GetOrderById(int id);
    }
}
