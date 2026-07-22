using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Auth
{
    public class JwtTokenResultDto
    {
        public string Token { get; init; } = string.Empty;

        public DateTimeOffset ExpiresAt { get; init; }
    }
}
