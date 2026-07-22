using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Enities;
using TaskFlow.Infrastructure.Persistence;
using Status = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByAssignedUserIdAsync(int userId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(x => x.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectIAsync(int projectId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(x => x.Deadline < DateTimeOffset.UtcNow && 
                x.Status != Status.Done)
                .ToListAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
