using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IAllOfficialVaccinationReposatory:IGenericReposatory<AllOfficialVaccination>
    {
        public Task<IEnumerable<AllOfficialVaccination>?> GetAllChildVaccinationAsync(CancellationToken cancellationToken);
        public Task<AllOfficialVaccination?> GetSpecificChildVaccinationAsync(int ID, CancellationToken cancellationToken);

    }
}
