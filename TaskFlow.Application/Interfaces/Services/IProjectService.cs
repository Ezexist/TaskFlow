using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Projects;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync(int userId, CreateProjectDto dto);

        Task<IEnumerable<ProjectResponseDto>> GetByUserIdAsync(int userId);

        Task<ProjectResponseDto> GetByIdAsync(int projectId, int userId);
        Task<ProjectResponseDto> UpdateAsync(
             int projectId,
             int userId,
             UpdateProjectDto dto);

        Task DeleteAsync(int projectId, int userId);
    }
}
