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
    public class ChildRepository : GenericRepository<Child>, IChildReposatory
    {
        private readonly ApplicationDbContext db;
        private readonly IParentReposatory parentReposatory;

        public ChildRepository(ApplicationDbContext db,IParentReposatory parentReposatory) : base(db)
        {
            this.db = db;
            this.parentReposatory = parentReposatory;
        }

        public async Task<Child?> GetByNationalIdAsync(string NationalId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.Children.Where(p => p.NationalId == NationalId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Child?> GetByParentIdAsync(int Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.Children.Where(p => p.ParentId == Id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Child>?> GetByParentNationalIdAsync(string ParentNationalId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var parent = await parentReposatory.GetByNationalIdAsync(ParentNationalId, cancellationToken);
            if (parent == null)
                return null;
            return await db.Children.Where(p => p.ParentId == parent.Id).ToListAsync(cancellationToken);
        }
    }
}
