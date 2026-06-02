using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class ParentDetailsResponse
    {
        public int ParentId { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } = null!;
        public string City { get; set; } = null!;

        #region Gender
        public string Gender { get; set; } = null!;
        #endregion

        #region Governorate
        public string Governorate { get; set; } = null!;

        #endregion

        #region BloodType
        public string BloodType { get; set; } = null!;
        #endregion

        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
