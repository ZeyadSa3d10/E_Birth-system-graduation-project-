using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.DoctorDtos
{
    public class AddUserMedicalRecord
    {
        public int DoctorId { get; set; }
        public int? ParentId { get; set; }
        public int? ChildId { get; set; }
        public string Description { get; set; } = null!;
        public string Medicine { get; set; } = null!;
        public IFormFile userMedicalImages { get; set; }
    }
}
