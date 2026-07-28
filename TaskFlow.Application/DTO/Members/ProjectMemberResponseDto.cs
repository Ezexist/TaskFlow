using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTO.Members
{
    public class ProjectMemberResponseDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public ProjectRole ProjectRole { get; set; }

        public DateTimeOffset JoinedAt { get; set; }
    }
}
