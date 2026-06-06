using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Configration
{
    public class DoctorConfigration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
            builder.Property(P => P.BirthDate).IsRequired();
            builder.Property(p => p.BloodType).IsRequired();
            builder.Property(p => p.Gender).IsRequired();
            builder.Property(p => p.Governorate).IsRequired();
            builder.Property(p => p.City).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Village).IsRequired().HasMaxLength(250);
            builder.Property(p => p.NationalId).IsRequired().HasMaxLength(14);
            builder.HasIndex(p => p.NationalId).IsUnique();
            builder.Property(p => p.FullName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.CreateAt).IsRequired();
            builder.Property(p => p.IsApproved).IsRequired();

            
            builder.HasOne(d => d.DoctorAttachment)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey<DoctorAttachment>(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.Gender)
                        .HasConversion<string>();

            builder.Property(p => p.Governorate)
                        .HasConversion<string>();

            builder.Property(p => p.BloodType)
                        .HasConversion<string>();

        }
    }
}
