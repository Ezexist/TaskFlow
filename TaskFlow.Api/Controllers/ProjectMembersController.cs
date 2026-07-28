using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Members;
using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/members")]

    public class ProjectMembersController : BaseController
    {
        private readonly IProjectMemberService _projectMemberService;
        public ProjectMembersController(IProjectMemberService projectMemberService)
        {
            _projectMemberService = projectMemberService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetMembers(int projectId)
        {
            var result = await _projectMemberService.GetMembersAsync(projectId, UserId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(int projectId, AddProjectMemberDto dto)
        {
            await _projectMemberService.AddMemberAsync(projectId, UserId, dto);
            return NoContent();
        }
        [HttpDelete("{memberId:int}")]
        public async Task<IActionResult> RemoveMember(int projectId, int memberId)
        {
            await _projectMemberService.RemoveMemberAsync(projectId, UserId, memberId);
            return NoContent();
        }
    }
}
