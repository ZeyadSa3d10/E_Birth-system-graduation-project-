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
    public class ChildVaccinationConfigration : IEntityTypeConfiguration<ChildVaccination>
    {
        public void Configure(EntityTypeBuilder<ChildVaccination> builder)
        {
            builder.HasKey(p => p.Id);




            builder.HasOne(p => p.Child)
                 .WithMany(v=>v.ChildVaccinations)
                 .HasForeignKey(p => p.ChildId)
                 .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(p => p.Hospital)
                   .WithMany(v=>v.ChildVaccinations)
                    .HasForeignKey(p => p.HospitalId)
                    .OnDelete(DeleteBehavior.Cascade);
           
            builder.HasOne(p => p.AllOfficialVaccination)
                   .WithMany(v => v.ChildVaccinations)
                    .HasForeignKey(p => p.AllOfficialVaccinationId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
