using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.DoctorDtos
{
    public class GetDoctorResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } = null!;
        public string City { get; set; } = null!;

        #region Gender
        public string Gender { get; set; }
        #endregion

        #region Governorate
        public string Governorate { get; set; }

        #endregion

        #region BloodType
        public string BloodType { get; set; }
        #endregion

        public DateTime CreateAt { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

      
    }
}
