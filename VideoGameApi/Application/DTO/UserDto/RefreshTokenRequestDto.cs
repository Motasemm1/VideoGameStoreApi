using System;

namespace VideoGameApi.Application.DTO.UserDto
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }

    }
}
 