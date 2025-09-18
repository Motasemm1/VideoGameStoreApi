using VideoGameApi.Application.DTO.UserDto;
using VideoGameApi.Domain;

namespace VideoGameApi.Application.Intefaces
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(RegisterDto request);
        Task <TokenResponseDto?>LoginAsync(LoginDto request);
        Task <TokenResponseDto?>RefreshTokenAsync(RefreshTokenRequestDto request);
        Task <AdminDto?>PromoteUserToAdminAsync (Guid id);
        Task <List<UserListDto>>GetAllUserAsync();
        Task <UserDetailsDto?> GetUserDetailsAsync (Guid id);
    }
}
