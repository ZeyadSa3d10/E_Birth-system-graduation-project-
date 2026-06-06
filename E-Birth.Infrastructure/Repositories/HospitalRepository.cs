using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Reposatories
{
    public class HospitalRepository : GenericRepository<Hospital>, IHospitalRepository
    {
        private readonly ApplicationDbContext db;

        public HospitalRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<Hospital?> GetByEmailAsync(string Email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.Hospitals.Where(p => p.Email == Email).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<Hospital?> GetByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.Hospitals.Where(p => p.PhoneNumber == PhoneNumber).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<int?> GetIdAsync(string Input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await db.Hospitals.Where(p => p.Email == Input || p.PhoneNumber == Input).FirstOrDefaultAsync(cancellationToken);
            if (result != null)
            {
                return result.Id;
            }
            return null;
        }
    }
}
