namespace VideoGameApi.Application.DTO.Orders
{
    public class UserSummaryDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int OrdersCount { get; set; }
        public List<int> OrderIds { get; set; } = new();

    }
}
