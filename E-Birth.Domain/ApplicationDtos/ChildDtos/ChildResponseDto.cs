using E_Birth.Domain.Enums;
using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ChildDtos
{
    public class ChildResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string ParentEmail { get; set; }
        public string ParentPhoneNumber { get; set; }
    }
}
