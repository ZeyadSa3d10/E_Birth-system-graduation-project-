using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class GetHospitalResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Governorate { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
