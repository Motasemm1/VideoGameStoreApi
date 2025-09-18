using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.UserDto
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
