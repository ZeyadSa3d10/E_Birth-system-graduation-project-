using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        #region Governorate
        public Governorate Governorate { get; set; }

        #endregion
        
        public DateTime CreateAt { get; set; }

        #region ChildVaccination
        public ICollection<ChildVaccination> ChildVaccinations { get; set; } = new List<ChildVaccination>();
        #endregion
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
    }
}
