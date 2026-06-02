using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ChildDtos
{
    public  class CreateChildDto
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public BloodType BloodType { get; set; }
        public Governorate Governorate { get; set; }
        public string City { get; set; } = null!;
        public string Village { get; set; } = null!;
        public string ParentNationalId { get; set; } = null!;
    }
}
