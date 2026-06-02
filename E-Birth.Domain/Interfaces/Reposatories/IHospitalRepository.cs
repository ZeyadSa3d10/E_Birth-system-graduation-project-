using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IHospitalRepository :IGenericReposatory<Hospital>
    {
        Task<int?> GetIdAsync(string Input, CancellationToken cancellationToken);
        Task<Hospital?> GetByEmailAsync(string Email, CancellationToken cancellationToken);
        Task<Hospital?> GetByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken);
    }
}
