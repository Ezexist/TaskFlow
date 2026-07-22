using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        JwtTokenResultDto GenerateAccessToken(User user);
    }
}
