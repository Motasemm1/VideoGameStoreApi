using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VideoGameApi.Application.DTO.Orders;
using VideoGameApi.Application.DTO.UserDto;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Domain;
using VideoGameApi.Domain.Enums;
using VideoGameApi.Infrastructure.Data;
using VideoGameApi.Infrastructure.Repositories;

namespace VideoGameApi.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public UserService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserName);
            if (user is null)
                return null;
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                return null;
            TokenResponseDto response = await CreateTokenResponse(user);
            return response;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }
        private string CreateToken(User user)
        {
            //claims
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            //convert security key to byte array
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSetting:Token"]!));

            //generate signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create token descriptor
            var tokenDescripter = new JwtSecurityToken(
                issuer: _config["AppSetting:Issuer"],//مين انشا التوكين
                audience: _config["AppSetting:Audience"],//مين هيستخدم التوكين
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),//تاريخ انتهاء التوكين
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescripter);
        }
        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var RefreshToken = GenerateRefreshToken();
            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);
            return RefreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> RegisterAsync(RegisterDto request)
        {
            if (await _userRepository.ExistsByUsernameAsync(request.UserName))
            {
                return null;
            }
            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.UserName = request.UserName;
            user.PasswordHash = hashedPassword;
            user.Role = UserRole.User;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.EmailConfirmed = false;

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;
            return await CreateTokenResponse(user);
        }

        public async Task<AdminDto?> PromoteUserToAdminAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return null;
            user.Role = UserRole.Admin;
            await _userRepository.Savechanges();
            var admin = new AdminDto();
            admin.UserName = user.UserName;
            admin.Role = user.Role;
            return admin;
        }
        public async Task<List<UserListDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users
       .Select(u => new UserListDto
       {
           UserId = u.UserId,
           UserName = u.UserName,
           Email = u.Email,
           Role = u.Role.ToString(),
           EmailConfirmed = u.EmailConfirmed,
           PhoneNumber = u.PhoneNumber
       })
       .ToList();
        }
        public async Task<UserDetailsDto?> GetUserDetailsAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                return null;

            return new UserDetailsDto
            {
                Id = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                EmailConfirmed = user.EmailConfirmed,
                VideoGames = user.UserVideoGames.Select(uv => new UserVideoGameDto
                {
                    GameId = uv.VideoGame.GameId,
                    GameTitle = uv.VideoGame.Title,
                    Genre = uv.VideoGame.Genre.Name,
                    Price = uv.VideoGame.Price,
                }).ToList(),
                Orders = user.Orders.Select(o => new CreateOrderDto
                {
                    Items = o.OrderItems.Select(oi => new CreateOrderItemDto
                    {
                        VideoGameId = oi.VideoGameId,
                        Quantity = oi.Quantity
                    }).ToList(),
                }).ToList(),
            };
        }
    }
}
