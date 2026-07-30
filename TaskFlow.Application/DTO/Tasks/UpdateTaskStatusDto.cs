using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.DTO.Tasks
{
    public class UpdateTaskStatusDto
    {
        public TaskStatus Status { get; set; }
    }
}
