using E_Birth.Domain.Models.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Configration
{
    public class OtpCodeConfigration : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.HashCode)
                   .IsRequired().HasMaxLength(500);

            builder.Property(o => o.ExpirationTime)
                   .IsRequired();

            builder.Property(o => o.CreateAt)
                   .IsRequired();

            builder.Property(o => o.IsUsed)
                   .IsRequired();

            builder.HasOne(o => o.User)
                   .WithMany(u => u.OtpCodes)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("OtpCodes");
        }
    }
}
