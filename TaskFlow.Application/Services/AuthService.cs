
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        public AuthService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
