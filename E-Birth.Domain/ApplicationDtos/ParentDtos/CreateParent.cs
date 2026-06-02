using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class CreateParent
    {
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

        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Passworded { get; set; } = null!;
        public string ConfirmPassworded { get; set; } = null!;
    }
}
