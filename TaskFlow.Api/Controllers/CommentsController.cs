using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Comments;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost("/api/tasks/{taskId:int}/comments")]
        public async Task<ActionResult<CommentResponseDto>> Create(int taskId, CreateCommentDto dto)
        {
            var result = await _commentService.CreateAsync(taskId, UserId, dto);
            return Ok(result);
        }
        [HttpGet("/api/tasks/{taskId:int}/comments")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetByTaskId(int taskId)
        {
            var result = await _commentService.GetByTaskAsync(taskId, UserId);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CommentResponseDto>> Update(int id,UpdateCommentDto dto)
        {
            var result = await _commentService.UpdateAsync(id,UserId,dto);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.DeleteAsync(
                id,
                UserId);

            return NoContent();
        }
    }
}
