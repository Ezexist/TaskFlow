using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Enities;
using TaskFlow.Infrastructure.Authentication;

namespace TaskFlow.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }
        public RefreshToken GenerateRefreshToken(int userId)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(bytes),
                UserId = userId,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationDays)
            }; 
        }
    }
}
