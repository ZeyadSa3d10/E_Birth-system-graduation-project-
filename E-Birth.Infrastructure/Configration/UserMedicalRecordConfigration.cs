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
    public class UserMedicalRecordConfigration : IEntityTypeConfiguration<UserMedicalRecord>
    {
        public void Configure(EntityTypeBuilder<UserMedicalRecord> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.Description).IsRequired();

           
            builder.HasOne(x => x.Parent)
                   .WithMany(p => p.UserMedicalRecords)
                   .HasForeignKey(x => x.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(x => x.child)
                   .WithMany(c => c.UserMedicalRecord)
                   .HasForeignKey(x => x.ChildId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Doctor)
                   .WithMany()
                   .HasForeignKey(x => x.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

          
          

        }
    }
}
