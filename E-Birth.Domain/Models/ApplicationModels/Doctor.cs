using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class Doctor 
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } = null!;
        public string City { get; set; } = null!;

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
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
     
        #region DoctorAttachment
        public DoctorAttachment DoctorAttachment { get; set; } = null!;
        #endregion

        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }

    }
}
