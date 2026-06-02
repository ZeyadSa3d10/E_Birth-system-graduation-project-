using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class ChildVaccination
    {
        public int Id { get; set; }
        public DateTime DateGiven { get; set; }

        // Navigation
        #region Child
        public Child Child { get; set; } = null!;
        public int ChildId { get; set; }
        #endregion

        #region Hospital
        public Hospital Hospital { get; set; } = null!;
        public int HospitalId { get; set; }
        #endregion

        #region ChildVaccination
        public AllOfficialVaccination AllOfficialVaccination { get; set; }
        public int AllOfficialVaccinationId { get; set; }
        #endregion
    }
}
