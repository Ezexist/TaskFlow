using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Enities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TaskFlowDbContext _context;

        public RefreshTokenRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
           await _context.SaveChangesAsync();
        }

        public async Task<List<RefreshToken>> GetActiveByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens
                .Where(x => x.UserId == userId &&
                !x.IsRevoked &&
                x.ExpiresAt > DateTimeOffset.UtcNow)
                .ToListAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
