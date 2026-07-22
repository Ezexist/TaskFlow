using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Enities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder
                .Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            builder
               .Property(x => x.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .IsRequired();
            builder
               .Property(x => x.Role)
               .IsRequired();


            builder
                .HasIndex(x => x.Email)
                .HasDatabaseName("IX_User_Email")
                .IsUnique();
            builder
                .HasIndex(x => x.UserName)
                .HasDatabaseName("IX_User_UserName")
                .IsUnique();

            
        } 
    }
}
