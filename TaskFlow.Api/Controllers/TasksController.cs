using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/tasks")]
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(int projectId, CreateTaskDto dto)
        {
            var result = await _taskService.CreateAsync(projectId, UserId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetByProject(int projectId)
        {
            var result = await _taskService.GetByProjectAsync(projectId, UserId);
            return Ok(result);
        }
        [HttpGet("/api/tasks/{taskId:int}")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int taskId)
        {
            var result = await _taskService.GetByIdAsync(taskId, UserId);

            return Ok(result);
        }
        [HttpPut("/api/tasks/{projectId:int}")]
        public async Task<ActionResult<TaskResponseDto>> Update(int projectId, UpdateTaskDto dto)
        {
            var result = await _taskService.UpdateAsync(projectId, UserId, dto);
            return Ok(result);
        }
        [HttpDelete("/api/tasks/{taskId:int}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            await _taskService.DeleteAsync(taskId, UserId);
            return NoContent();
        }
        [HttpPut("/api/tasks/{taskId:int}/status")]
        public async Task<ActionResult<TaskResponseDto>> UpdateStatus(int taskId, UpdateTaskStatusDto dto)
        {
            var result = await _taskService.UpdateStatusAsync(taskId, UserId, dto);

            return Ok(result);
        }

        [HttpPut("/api/tasks/{id:int}/assign")]
        public async Task<ActionResult<TaskResponseDto>> AssignUser(int id,AssignTaskDto dto)
        {
            var result = await _taskService.AssignUserAsync(id, UserId, dto);

            return Ok(result);
        }
        [HttpGet("/api/tasks/my")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var result = await _taskService.GetMyTasksAsync(UserId);
            return Ok(result);
        }
        [HttpGet("api/tasks/overdue")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetOverdueTasks()
        {
            var result = await _taskService.GetOverdueTasksAsync(UserId);
            return Ok(result);
        }
    }
}
