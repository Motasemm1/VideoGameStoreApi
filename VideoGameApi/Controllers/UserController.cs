using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Application.DTO.UserDto;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Domain;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _authService;
        public UserController(IUserService authService)
        {
            _authService = authService;
        }
        [Authorize(Roles="Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserListDto>>> GetAllUsers()
        {
            var users = await _authService.GetAllUserAsync();
            if (users is null || users.Count == 0)
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("User already exists");
            }
            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response is null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("TestTokenAfterLogin")]
        public IActionResult TestTokenAfterLogin()
        {
            return Ok("You are authenticated");
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequestDto);
            if (result is null)
                return Unauthorized("Invalid Refresh Token");
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("promote/{id}")]
        public async Task<ActionResult> PromoteUser(Guid id)
        {
            var admin = await _authService.PromoteUserToAdminAsync(id);
            if (admin == null)
                return NotFound("User not found");

            return Ok($"User {admin.UserName} promoted to Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserDetails/{id}")]
        public async Task<ActionResult> GetUserDetailsById(Guid id)
        {
            var userDetails = await _authService.GetUserDetailsAsync(id);
            if (userDetails == null)
                return NotFound("User not found");

            return Ok(userDetails);
        }
    }

}