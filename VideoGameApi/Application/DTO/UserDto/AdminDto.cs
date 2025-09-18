using VideoGameApi.Domain.Enums;

namespace VideoGameApi.Application.DTO.UserDto
{
    public class AdminDto
    {
        public string UserName { get; set; }
        public UserRole Role { get; set; }
    }
}
