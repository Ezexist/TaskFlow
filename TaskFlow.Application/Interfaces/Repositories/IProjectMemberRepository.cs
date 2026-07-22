using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IProjectMemberRepository
    {
        Task<ProjectMember?> GetByIdAsync(int id);
        Task<bool> IsProjectMemberAsync(int projectId, int useId);
        Task AddAsync(ProjectMember projectMember);
        Task DeleteAsync(ProjectMember projectMember);
    }
}
