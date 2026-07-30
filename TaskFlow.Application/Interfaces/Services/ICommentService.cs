using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Comments;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentResponseDto> CreateAsync( int taskId,int userId,CreateCommentDto dto);

        Task<IEnumerable<CommentResponseDto>> GetByTaskAsync(int taskId,int userId);

        Task<CommentResponseDto> UpdateAsync(int commentId,int userId,UpdateCommentDto dto);

        Task DeleteAsync( int commentId, int userId);
    }
}
