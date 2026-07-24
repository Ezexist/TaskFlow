using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);

        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<List<RefreshToken>> GetActiveByUserIdAsync(int userId);

        Task SaveChangesAsync();

        Task DeleteAsync(RefreshToken refreshToken);
    }
}
