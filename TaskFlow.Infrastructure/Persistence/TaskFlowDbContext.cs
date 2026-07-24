using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Enities;


namespace TaskFlow.Infrastructure.Persistence
{
    public class TaskFlowDbContext : DbContext
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) 
            : base(options)
        {        
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskFlowDbContext).Assembly);
        }
    }
}
