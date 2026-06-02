using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class AddParentFromHospitalDto
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } = null!;
        public string City { get; set; } = null!;

        public Gender Gender { get; set; }

        public Governorate Governorate { get; set; }


        public BloodType BloodType { get; set; }

        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        
    }
}
