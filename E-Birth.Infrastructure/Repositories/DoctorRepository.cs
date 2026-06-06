using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorReposatory
{
    private readonly ApplicationDbContext db;

    public DoctorRepository(ApplicationDbContext db) : base(db)
    {
        this.db = db;
    }

    public async Task<Doctor?> GetByEmailAsync(string Email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await db.Doctors.Where(p => p.Email == Email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByNationalIdAsync(string NationalId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await db.Doctors.Where(p => p.NationalId == NationalId).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Doctor?> GetByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await db.Doctors.Where(p => p.PhoneNumber == PhoneNumber).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<int?> GetIdAsync(string Input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await db.Doctors.Where(p => p.Email == Input || p.NationalId == Input).FirstOrDefaultAsync(cancellationToken);
        if (result != null)
        {
            return result.Id;
        }
        return null;
    }
}
