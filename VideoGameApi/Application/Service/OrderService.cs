using Microsoft.EntityFrameworkCore;
using System.Linq;
using VideoGameApi.Application.DTO.Orders;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;
using VideoGameApi.Infrastructure.Repositories;

namespace VideoGameApi.Application.Service
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IVideoGameRepository _videoGameRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IVideoGameRepository videoGameRepository, IUserRepository userRepository)
        {
            _videoGameRepository = videoGameRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }
        public async Task<orderDto> CreateOrderAsync(Guid userId, CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };
            decimal total = 0;

            foreach (var item in createOrderDto.Items)
            {

                var game = await _videoGameRepository.GetByIdAsync(item.VideoGameId);
                if (game == null)
                    return null;
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    VideoGameId = game.GameId,
                    UnitPrice = game.Price,
                    Quantity = item.Quantity,
                };
                total += game.Price * item.Quantity;

                order.OrderItems.Add(orderItem);

               var exist = await _videoGameRepository.UserVideoGameExist(order.UserId, item.VideoGameId);
                if (!exist)
                {
                    var uservideogame = new UserVideoGame
                    {
                        UserId = userId,
                        GameId = game.GameId
                    };
                    await _videoGameRepository.AddToUserVideoGame(uservideogame);
                }
            }
            order.TotalAmount = total;
            order.OrderDate = DateTime.UtcNow;

            await _orderRepository.AddAsync(order);

            return new orderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    VideoGameId = i.VideoGameId,
                    GameTitle = i.VideoGame.Title,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task<ICollection<UserSummaryDto>> GetAllOrders()
        {
            var users = await _userRepository.GetAllAsync();
            return users
                 .Select(u => new UserSummaryDto
                 {
                     UserId = u.UserId,
                     UserName = u.UserName,
                     OrdersCount = u.Orders.Count,
                     OrderIds = u.Orders.Select(o => o.Id).ToList()
                 })
                 .ToList();
        }

        public async Task<OrderDetailsDto?> GetOrderById(int id)
        {
            var orderDetails = await _orderRepository.GetByIdAsync(id);
            if (orderDetails == null)
                return null;
            return new OrderDetailsDto
            {
                Id = orderDetails.Id,
                userId = orderDetails.UserId,
                userName = orderDetails.User.UserName,
                OrderDate = orderDetails.OrderDate,
                TotalAmount = orderDetails.TotalAmount,
                Items = orderDetails.OrderItems.Select(oi => new OrderItemDto
                {
                    GameTitle = oi.VideoGame.Title,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    VideoGameId = oi.VideoGameId,

                }).ToList()
            };

        }

        public async Task<UserOrdersResponseDto> GetUserOrders(Guid userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            var userName = user != null ? user.UserName : "Unknown User";
            var result = new UserOrdersResponseDto
            {
                UserName = userName,
                OrdersCount = orders.Count(),
                Orders = orders.Select(o => new orderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        VideoGameId = oi.VideoGameId,
                        GameTitle = oi.VideoGame.Title,
                        UnitPrice = oi.UnitPrice,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToList()
            };
            return result;
        }
    }
}
