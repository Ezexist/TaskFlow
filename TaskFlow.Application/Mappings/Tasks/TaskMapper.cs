using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Mappings.Tasks
{
    public static class TaskMapper
    {
        public static TaskResponseDto ToResponseDto(this TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                CreatedByUserName = task.CreatedBy.UserName,
                AssignedUserName = task.AssignedUser?.UserName
            };
        }
    }
}
