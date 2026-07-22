using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(x => x.Description)
                .HasMaxLength(2000);

            builder
                .Property(x => x.Status)
                .IsRequired();
            builder
                .Property(x => x.Priority)
                .IsRequired();
            builder
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.HasOne(x => x.Project)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ProjectId)
                .HasConstraintName("FK_Task_Project_ProjectId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedTasks)
                .HasForeignKey(x => x.CreatedById)
                .HasConstraintName("FK_Task_CreatedBy_CreatedById")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedUser)
                .WithMany(x => x.AssignedTasks)
                .HasForeignKey(x => x.AssignedUserId)
                .HasConstraintName("FK_Task_AssignedUser_AssignedUserId")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.TaskItem)
                .HasForeignKey(x => x.TaskItemId)
                .HasConstraintName("FK_Task_Comment_TaskItemId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
