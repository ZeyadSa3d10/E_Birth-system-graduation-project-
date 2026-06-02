using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class ChildVaccinationsResponse
    {
        public int ChildId { get; set; }
        public string Gender { get; set; } = null!;
        public int ChildAgeInMonths { get; set; }
        public int ChildAgeInYears { get; set; }
      
        public List<SpecificVaccinate>? Vaccinations { get; set; } = new List<SpecificVaccinate>();

    }
    public class SpecificVaccinate 
    {
        public int IdForSpecificVaccinateFromAll { get; set; }
        public string ChildVaccinationsName { get; set; } = null!;
        public DateOnly OfficialDate { get; set; }
        public DateTime? TakenDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
