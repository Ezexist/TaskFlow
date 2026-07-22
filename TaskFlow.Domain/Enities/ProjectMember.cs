using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Enities
{
    public class ProjectMember
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTimeOffset JoinedAt { get; set; }
        public ProjectRole ProjectRole { get; set; }

    }
}
