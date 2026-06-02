using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IDoctorReposatory :IGenericReposatory<Doctor>
    {
        Task<Doctor?> GetByNationalIdAsync(string NationalId, CancellationToken cancellationToken);
        Task<int?> GetIdAsync(string Input, CancellationToken cancellationToken);
        Task<Doctor?> GetByEmailAsync(string Email, CancellationToken cancellationToken);
        Task<Doctor?> GetByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken);

    }
}
