using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.DTO.Tasks
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskStatus Status { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTimeOffset? Deadline { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedByUserName { get; set; } = string.Empty;

        public string? AssignedUserName { get; set; }
    }
}
