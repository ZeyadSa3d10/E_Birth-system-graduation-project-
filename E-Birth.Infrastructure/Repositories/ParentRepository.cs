using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace E_Birth.Infrastructure.Repositories;

public class ParentRepository : GenericRepository<Parent>, IParentReposatory
{
    private readonly ApplicationDbContext db;

    public ParentRepository(ApplicationDbContext db) : base(db)
    {
        this.db = db;
    }

    public async Task<Parent?> GetByEmailAsync(string Email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
       return await db.Parents.Where(p => p.Email == Email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Parent?> GetByNationalIdAsync(string NationalId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await db.Parents.Where(p => p.NationalId == NationalId).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Parent?> GetByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await db.Parents.Where(p => p.PhoneNumber == PhoneNumber).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int?> GetIdAsync(string Input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await db.Parents.Where(p => p.Email == Input || p.NationalId==Input).FirstOrDefaultAsync(cancellationToken);
        if (result != null)
        {
            return result.Id;
        }
        return null;
    }
}
