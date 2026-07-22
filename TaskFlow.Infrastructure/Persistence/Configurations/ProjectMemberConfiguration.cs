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
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.ToTable("ProjectMembers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(x => x.ProjectRole)
                .IsRequired();

            builder.HasOne(x => x.Project)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.ProjectId)
                .HasConstraintName("FK_ProjectMembers_Members_ProjectId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.ProjectMemberships)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_ProjectMembers_ProjectMemberships_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.ProjectId,
                x.UserId
            })
                .HasDatabaseName("IX_ProjectMembers_ProjectId_UserId")
                .IsUnique();

        }
    }
}
