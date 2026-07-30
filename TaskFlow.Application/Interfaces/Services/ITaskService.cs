using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Tasks;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateAsync(
            int projectId,
            int userId,
            CreateTaskDto dto);
       Task<IEnumerable<TaskResponseDto>> GetByProjectAsync(
          int projectId,
          int userId);

        Task<TaskResponseDto> UpdateAsync(
          int taskId,
          int userId,
          UpdateTaskDto dto);
        Task DeleteAsync(int taskId, int userId);
        Task<TaskResponseDto> GetByIdAsync(int taskId, int userId);
        Task<TaskResponseDto> UpdateStatusAsync(int taskId,int userId,UpdateTaskStatusDto dto);

        Task<TaskResponseDto> AssignUserAsync(int taskId,int userId, AssignTaskDto dto);
        Task<IEnumerable<TaskResponseDto>> GetMyTasksAsync(int userId);
        Task<IEnumerable<TaskResponseDto>> GetOverdueTasksAsync(int userId);
    }
}
