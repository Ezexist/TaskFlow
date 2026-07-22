using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Enities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskFlowDbContext _context;

        public ProjectRepository(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(x  => x.Id == id);
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(int userId)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
