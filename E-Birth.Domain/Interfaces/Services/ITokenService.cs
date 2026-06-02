using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(ApplicationUser applicationUser, CancellationToken ct);
    }
}
