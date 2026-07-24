using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        RefreshToken GenerateRefreshToken(int userId);
    }
}
