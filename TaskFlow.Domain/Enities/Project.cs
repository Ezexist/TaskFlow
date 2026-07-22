using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Enities
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public ICollection<TaskItem> Tasks { get; set; } = [];
        public ICollection<ProjectMember> Members { get; set; } = [];
    }
}
