using VideoGameApi.Application.DTO.Orders;

namespace VideoGameApi.Application.DTO.UserDto
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }   
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; } 
        public List<UserVideoGameDto> VideoGames { get; set; } 
        public List<CreateOrderDto> Orders { get; set; } 
    }
}
