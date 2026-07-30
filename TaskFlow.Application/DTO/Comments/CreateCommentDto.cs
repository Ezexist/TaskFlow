using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO.Comments
{
    public class CreateCommentDto
    {
        public required string Message { get; set; }
    }
}
