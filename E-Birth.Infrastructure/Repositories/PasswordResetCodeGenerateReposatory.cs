using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Repositories
{
    public class PasswordResetCodeGenerateReposatory : GenericRepository<OtpCode>, IPasswordResetCodeGenerateReposatory
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public PasswordResetCodeGenerateReposatory(ApplicationDbContext db,UserManager<ApplicationUser> userManager) : base(db)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<OtpCode> GetLastCodeByEmail(string email, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return null;
            cancellationToken.ThrowIfCancellationRequested();
            return await db.OtpCodes
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreateAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
