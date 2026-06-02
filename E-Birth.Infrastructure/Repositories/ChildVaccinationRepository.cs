using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Reposatories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Infrastructure.Repositories
{
    public class ChildVaccinationRepository : GenericRepository<ChildVaccination>, IChildVaccinationRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IChildReposatory childReposatory;
        private readonly IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory;

        public ChildVaccinationRepository(ApplicationDbContext db,
                                           IChildReposatory childReposatory,
                                           IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory
                                           ) : base(db)
        {
            this.db = db;
            this.childReposatory = childReposatory;
            this.allOfficialVaccinationReposatory = allOfficialVaccinationReposatory;
        }
        public async Task<int> GetLateVaccinationsCountAsync(CancellationToken cancellationToken)
        {
            var sixteenYearsAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-16));

            var children = await childReposatory.GetAllAsync(
                c => c.BirthDate > sixteenYearsAgo,
                cancellationToken);

            var officialVaccinations = await allOfficialVaccinationReposatory.GetAllAsync(cancellationToken);
            var childVaccinations = await GetAllAsync(cancellationToken);

            int count = 0;

            foreach (var child in children)
            {
                var ageMonths =
                    (DateTime.UtcNow.Year - child.BirthDate.Year) * 12 +
                    DateTime.UtcNow.Month - child.BirthDate.Month;

                var expected = officialVaccinations.Count(v => v.AgeInMonths <= ageMonths);
                var taken = childVaccinations.Count(v => v.ChildId == child.Id);

                if (taken < expected)
                    count++;
            }

            return count;
        }

        public async Task<IEnumerable<Child>> GetChildrenWithLateVaccinationsAsync(CancellationToken cancellationToken)
        {
            var sixteenYearsAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-16));

            var children = await childReposatory.GetAllAsync(
                c => c.BirthDate > sixteenYearsAgo,
                cancellationToken);

            var officialVaccinations = await allOfficialVaccinationReposatory.GetAllAsync(cancellationToken);
            var childVaccinations = await GetAllAsync(cancellationToken);

            var lateChildren = new List<Child>();

            foreach (var child in children)
            {
                var ageMonths =
                    (DateTime.UtcNow.Year - child.BirthDate.Year) * 12 +
                    DateTime.UtcNow.Month - child.BirthDate.Month;

                var expected = officialVaccinations.Count(v => v.AgeInMonths <= ageMonths);
                var taken = childVaccinations.Count(v => v.ChildId == child.Id);

                if (taken < expected)
                    lateChildren.Add(child);
            }

            return lateChildren;
        }

    }
}
