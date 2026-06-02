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
    public class MedicalRecordImageConfigration : IEntityTypeConfiguration<MedicalRecordImage>
    {
        public void Configure(EntityTypeBuilder<MedicalRecordImage> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ImagePath).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired();


            builder.HasOne(p => p.UserMedicalRecord)
                   .WithMany(U=>U.MedicalRecordImages)
                   .HasForeignKey(p => p.UserMedicalRecordId)
                   .OnDelete(DeleteBehavior.Cascade);
                

        }

    }
}
