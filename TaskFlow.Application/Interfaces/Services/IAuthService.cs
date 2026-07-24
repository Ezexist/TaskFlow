using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto);
        Task LogoutAsync(string refreshToken);
    }
}
