using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IChildVaccinationRepository :IGenericReposatory<ChildVaccination>
    {
        Task<int> GetLateVaccinationsCountAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Child>> GetChildrenWithLateVaccinationsAsync(CancellationToken cancellationToken);
    }
}
