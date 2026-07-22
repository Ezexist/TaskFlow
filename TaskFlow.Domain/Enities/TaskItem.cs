using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Domain.Enities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;

        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = [];
    }
}
