namespace VideoGameApi.Application.DTO.UserDto
{
    public class UserListDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }    
    }
}
