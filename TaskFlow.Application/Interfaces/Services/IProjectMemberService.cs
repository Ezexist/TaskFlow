using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Members;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IProjectMemberService
    {
        Task<IEnumerable<ProjectMemberResponseDto>> GetMembersAsync(int projectId, int userId);

        Task AddMemberAsync(int projectId, int userId, AddProjectMemberDto dto);
        Task RemoveMemberAsync(int projectId, int userId,int memberId);
    }
}
