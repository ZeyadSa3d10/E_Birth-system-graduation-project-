using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class UpdateChildVaccinationsDto
    {
        public int ChildId { get; set; }
        public int HospitalId { get; set; }
        public int SpecificVaccinationId { get; set; }
    }
}
