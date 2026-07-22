using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Enities
{
    public class User
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        public ICollection<Project> OwnedProjects {  get; set; } = [];
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = [];
        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        
    }

}

