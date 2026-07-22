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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(2000);

            builder
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.HasOne(x => x.TaskItem)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.TaskItemId)
                .HasConstraintName("FR_Comment_Tasks_TaskItemId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FR_Comment_User_UserId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
