namespace VideoGameApi.Domain
{
    public class OrderItem
    {
        public int Id { get; set; } // PK

        // FK to Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // FK to VideoGame
        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
