namespace VideoGameApi.Domain
{
    public class UserVideoGame
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public VideoGame VideoGame { get; set; }
    }
}
