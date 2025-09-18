namespace VideoGameApi.Domain
{
    public class Order
    {
        public int Id { get; set; }  // OrderId (PK)

        // Foreign Key to User
        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation Property
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}