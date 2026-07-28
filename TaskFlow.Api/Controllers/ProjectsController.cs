using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Services;
using System.Security.Claims;
using TaskFlow.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto dto)
        {
            var result = await _projectService.CreateAsync(UserId, dto);

            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetMyProjects()
        {

            var result = await _projectService.GetByUserIdAsync(UserId);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
        {


            var result = await _projectService.GetByIdAsync(id, UserId);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProjectResponseDto>> Update(int id ,UpdateProjectDto dto)
        {
            var result = await _projectService.UpdateAsync(id, UserId, dto);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteAsync(id,UserId);
            return NoContent();
        }

    }
}
