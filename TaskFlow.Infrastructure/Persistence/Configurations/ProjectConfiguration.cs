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
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.Description)
                .HasMaxLength(1000);

            builder
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.OwnedProjects)
                .HasForeignKey(x => x.OwnerId)
                .HasConstraintName("FK_Projects_Users_OwnerId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .HasConstraintName("FK_Projects_Tasks_OwnerId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Members)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .HasConstraintName("FK_Projects_Members_OwnerId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
