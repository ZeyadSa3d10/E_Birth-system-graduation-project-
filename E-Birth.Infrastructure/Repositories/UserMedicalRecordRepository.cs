using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Repositories
{
    public class UserMedicalRecordRepository : GenericRepository<UserMedicalRecord>, IUserMedicalRecordRepository
    {
        public UserMedicalRecordRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
