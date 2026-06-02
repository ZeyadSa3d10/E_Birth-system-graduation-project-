using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class AddChildVaccinationRequest
    {
            public int ChildId { get; set; }
            public string Gender { get; set; } = null!;
            public int ChildAgeInMonths { get; set; }
            public int ChildAgeInYears { get; set; }

            public List<UpdateSpecificVaccinate>? Vaccinations { get; set; } = new List<UpdateSpecificVaccinate>();
    }
    public class UpdateSpecificVaccinate
    {
        public string ChildVaccinationsName { get; set; } = null!;
        public DateOnly OfficialDate { get; set; }
        public DateTime? TakenDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
