using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Auth
{
    public class LoginDto
    {
        [DefaultValue("gena@gmail.com")]
        public string Email { get; set; } = string.Empty;
        [DefaultValue("gena38GENA")]
        public string Password { get; set; } = string.Empty;
    }
}
