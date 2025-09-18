namespace VideoGameApi.Application.DTO.Orders
{
    public class UserVideoGameDto
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}
