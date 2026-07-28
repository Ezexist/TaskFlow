using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTO.Tasks
{
    public class CreateTaskDto
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTimeOffset? Deadline { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
