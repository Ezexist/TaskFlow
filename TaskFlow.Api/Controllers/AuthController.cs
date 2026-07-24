using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshAsync(dto);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken); 
            return NoContent();
        }

    }
}
