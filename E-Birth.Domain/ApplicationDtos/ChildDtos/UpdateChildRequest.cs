using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ChildDtos
{
    public class UpdateChildRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public BloodType BloodType { get; set; }
        public Governorate Governorate { get; set; }
        public string Village { get; set; }
        public string City { get; set; }
        public string ParentNationalId { get; set; }
    }
}
