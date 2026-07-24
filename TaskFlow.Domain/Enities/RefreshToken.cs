using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Enities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public bool IsExpired => ExpiresAt <= DateTimeOffset.UtcNow;
        public bool IsActive => !IsRevoked && !IsExpired;


        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
