using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class Child 
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; }
        public string City { get; set; }

        #region Gender
        public Gender Gender { get; set; }
        #endregion

        #region Governorate
        public Governorate Governorate { get; set; }

        #endregion

        #region BloodType
        public BloodType BloodType { get; set; }
        #endregion

        public DateTime CreateAt { get; set; }

        #region Parent
        public Parent Parent { get; set; } = null!;
        public int ParentId { get; set; }
        #endregion


        #region ChildVaccination
        public ICollection<ChildVaccination> ChildVaccinations { get; set; } = null!;
        #endregion

        #region UserMedicalRecord
        public ICollection<UserMedicalRecord> UserMedicalRecord { get; set; } = new List<UserMedicalRecord>();
        #endregion
        public bool IsDeleted { get; set; }
    }
}
