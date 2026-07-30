using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Comments;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Application.Mappings.Comments
{
    public static class CommentMapper 
    {
        public static CommentResponseDto ToResponseDto(
            this Comment comment)
        {
            return new CommentResponseDto
            {
                Id = comment.Id,
                Message = comment.Message,
                CreatedAt = comment.CreatedAt,
                UserName = comment.User.UserName
            };
        }
    }
}
