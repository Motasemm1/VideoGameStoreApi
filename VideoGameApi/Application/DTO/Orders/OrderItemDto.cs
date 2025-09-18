namespace VideoGameApi.Application.DTO.Orders
{
    public class OrderItemDto
    {
        public int VideoGameId { get; set; }
        public string GameTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
