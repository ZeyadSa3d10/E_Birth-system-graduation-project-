using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class UpdateParent
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } =null!;
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
    }
}
