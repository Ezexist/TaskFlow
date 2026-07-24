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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.Property(x => x.ExpiresAt)
          .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.User.Id)
                .HasConstraintName("FK_RefreshToken_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
