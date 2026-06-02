using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class CreateHospital
    {
        public string Name { get; set; } = null!;
        public double Longitude { get; set; }
        public double Latitude { get; set; } 
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        #region Governorate
        public Governorate Governorate { get; set; }

        #endregion

        public string Passworded { get; set; } = null!;
        public string ConfirmPassworded { get; set; } = null!;
    }
}
