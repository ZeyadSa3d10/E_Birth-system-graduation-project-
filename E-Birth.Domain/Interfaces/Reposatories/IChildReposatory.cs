using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IChildReposatory:IGenericReposatory<Child>
    {
        Task<Child?> GetByNationalIdAsync(string NationalId, CancellationToken cancellationToken);
        Task<Child?> GetByParentIdAsync(int Id, CancellationToken cancellationToken);
        Task<IEnumerable<Child>?> GetByParentNationalIdAsync(string ParentNationalId, CancellationToken cancellationToken);
    }
}
