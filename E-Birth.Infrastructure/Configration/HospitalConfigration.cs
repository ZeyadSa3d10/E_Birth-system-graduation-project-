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
    public class HospitalConfigration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasIndex(h => h.Email).IsUnique();

           
            builder.Property(p => p.Governorate)
                        .HasConversion<string>();

          
        }
    }
}
