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
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedUser)
                .Where(x => x.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(x => x.AssignedUser)
                .Include(x => x.CreatedBy)
                .Where(p => p.ProjectId == projectId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetDetailsByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(x => x.AssignedUser)
                .Include(x => x.Project)
                    .ThenInclude(x => x.Members)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(int userId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedUser)
                .Where(x =>
                x.Deadline < DateTimeOffset.UtcNow &&
                x.Status != Status.Done &&
                x.Project.Members.Any(x => x.UserId == userId))
                .ToListAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
