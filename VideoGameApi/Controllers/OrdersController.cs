using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VideoGameApi.Application.DTO.Orders;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("CreateOrder")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.CreateOrderAsync(Guid.Parse(userId), dto);
            if (order == null)
                return NotFound("Game Not Found");
            return Ok(order);
        }
        [HttpGet("GetUserOrders")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrders(Guid.Parse(userId));
            if (orders == null)
                return NotFound("There is no Orders");
            return Ok(orders);
        }
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            if (orders == null)
                return NotFound("There is no Orders");
            return Ok(orders);
        }
        [HttpGet("GetOrderById{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
                return NotFound("Order Not Found");
            return Ok(order);
        }

    }

}
