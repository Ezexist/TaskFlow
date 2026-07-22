using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Enities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
