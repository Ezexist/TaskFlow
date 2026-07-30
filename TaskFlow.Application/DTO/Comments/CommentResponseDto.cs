using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Comments
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
    }
}
