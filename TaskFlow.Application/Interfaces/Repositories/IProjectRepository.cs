using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<IEnumerable<Project>> GetByUserIdAsync(int userId);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
