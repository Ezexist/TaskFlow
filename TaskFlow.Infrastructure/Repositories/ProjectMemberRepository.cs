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
    public class ProjectMemberRepository :IProjectMemberRepository
    {
        private readonly TaskFlowDbContext _context;

        public ProjectMemberRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectMember projectMember)
        {
            _context.ProjectMembers.Remove(projectMember);

            await _context.SaveChangesAsync();
        }

        public async Task<ProjectMember?> GetByIdAsync(int id)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.Id == id);
        }

        public async Task<bool> IsProjectMemberAsync(int projectId, int useId)
        {
            return await _context.ProjectMembers
                .AnyAsync(pm => projectId == pm.Id &&
                pm.UserId == useId);
        }
    }
}
