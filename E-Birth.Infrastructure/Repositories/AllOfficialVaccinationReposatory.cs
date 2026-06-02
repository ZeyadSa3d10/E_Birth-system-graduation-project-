using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Repositories
{
    public class AllOfficialVaccinationReposatory : GenericRepository<AllOfficialVaccination>, IAllOfficialVaccinationReposatory
    {
        private readonly ApplicationDbContext db;

        public AllOfficialVaccinationReposatory(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AllOfficialVaccination>?> GetAllChildVaccinationAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.AllOfficialVaccinations.ToListAsync(cancellationToken);
        }

        public async Task<AllOfficialVaccination?> GetSpecificChildVaccinationAsync(int ID, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.AllOfficialVaccinations.FirstOrDefaultAsync(v => v.Id == ID, cancellationToken);
        }
    }
}
