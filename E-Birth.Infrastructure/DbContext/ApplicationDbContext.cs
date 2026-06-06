using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.Configration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorAttachment> DoctorAttachments { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Child> Children { get; set; }
    public DbSet<AllOfficialVaccination> AllOfficialVaccinations { get; set; }
    public DbSet<ChildVaccination> ChildVaccinations { get; set; }
    public DbSet<MedicalRecordImage> MedicalRecordImages { get; set; }
    public DbSet<UserMedicalRecord> UserMedicalRecords { get; set; }
    public DbSet<AllOfficialVaccination> VaccinationSchedule { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AssemplyForApplyConfigrations).Assembly);
        base.OnModelCreating(builder);
    }
    
}
