using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class UserMedicalHistoryResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; } = null!;
        public int UserAgeInMonths { get; set; }
        public int UserAgeInYears { get; set; }
        public List<UserDetails>? MedicalHistory { get; set; } = new List<UserDetails>();
    }
    public class UserDetails
    {
        public int MedicalRecordId { get; set; }
        public string DoctorName { get; set; } = null!;
        public DateTime GivenAt { get; set; }
        public string Description { get; set; } = null!;
        public string Medicine { get; set; } = null!;
    }
}
