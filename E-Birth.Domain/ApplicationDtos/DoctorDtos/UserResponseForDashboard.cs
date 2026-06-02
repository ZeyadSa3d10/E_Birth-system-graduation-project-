using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.DoctorDtos
{
    public class UserResponseForDashboard
    {
       public string Type { get; set; } = null!;
       public ChildDetailsResponse? ChildDetails { get; set; }
       public ParentDetailsResponse? ParentDetails { get; set; }
    }
}
