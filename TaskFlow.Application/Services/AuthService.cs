
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Enities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService)
        {
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(
                dto.Password,
                user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var activeToken = await _refreshTokenRepository.GetActiveByUserIdAsync(user.Id);
            foreach (var token in activeToken)
            {
                token.IsRevoked = true;
            }
            await _refreshTokenRepository.SaveChangesAsync();

            var jwtResult = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = jwtResult.Token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = jwtResult.ExpiresAt
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if(token is null)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }
            if (!token.IsActive)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }
            token.IsRevoked = true ;

            await _refreshTokenRepository.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);
            if (refreshToken == null)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }
            if (!refreshToken.IsActive)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }
            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.SaveChangesAsync();

            var user = refreshToken.User;

            var jwtResult = _jwtTokenService.GenerateAccessToken(user);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new AuthResponseDto
            {
                AccessToken = jwtResult.Token,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = jwtResult.ExpiresAt
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if(await _userRepo.ExistsByEmailAsync(dto.Email))
            {
                throw new ConflictException("Email is already in use ");
            }
            if(await _userRepo.ExistsByUserNameAsync(dto.UserName))
            {
                throw new ConflictException("Username is already in use");
            }

            var passwordHash = _passwordHasher.HashPassword(dto.Password);

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = Role.User

            };
            await _userRepo.AddAsync(user);

            //var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var jwtResult = _jwtTokenService.GenerateAccessToken(user);

            var refsherToken = _refreshTokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepository.AddAsync(refsherToken);

            return new AuthResponseDto
            {
                AccessToken = jwtResult.Token,
                RefreshToken = refsherToken.Token,
                ExpiresAt = jwtResult.ExpiresAt
            };
        }
    }
}
