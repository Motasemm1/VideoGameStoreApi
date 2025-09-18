namespace VideoGameApi.Application.DTO.Orders
{
    public class UserOrdersResponseDto
    {
        public string UserName { get; set; }
        public int OrdersCount { get; set; }
        public List<orderDto> Orders { get; set; }
    }
}
