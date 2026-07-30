using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem?> GetDetailsByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<TaskItem>> GetByAssignedUserIdAsync(int userId);

        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(int userId);

        Task AddAsync(TaskItem task);

        Task UpdateAsync(TaskItem task);

        Task DeleteAsync(TaskItem task);
    }
}
