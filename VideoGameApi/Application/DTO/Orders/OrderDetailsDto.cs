namespace VideoGameApi.Application.DTO.Orders
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public Guid userId { get; set; }
        public string userName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDto> Items { get; set; }

    }
}
