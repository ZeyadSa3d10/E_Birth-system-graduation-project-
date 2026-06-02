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
    public class AllOfficialVaccinationConfigration : IEntityTypeConfiguration<AllOfficialVaccination>
    {
        public void Configure(EntityTypeBuilder<AllOfficialVaccination> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.VaccinationType).IsRequired();
            builder.Property(p => p.AgeInMonths).IsRequired();
        }
    
    }
}
