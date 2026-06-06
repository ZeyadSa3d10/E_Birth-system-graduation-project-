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
    public class DoctorAttachmentRepository : GenericRepository<DoctorAttachment>, IDoctorAttachmentRepository
    {
        private readonly ApplicationDbContext db;

        public DoctorAttachmentRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<string> GetByDoctorIdAsync(int Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await db.DoctorAttachments.Where(p => p.DoctorId == Id).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return null;
            return result.FilePath;
        }
    }
}
