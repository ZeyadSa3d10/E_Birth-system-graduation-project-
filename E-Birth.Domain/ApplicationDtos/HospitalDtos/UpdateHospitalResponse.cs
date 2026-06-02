using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class UpdateHospitalResponse
    {
        public string? Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string Governorate { get; set; }
    }
}
