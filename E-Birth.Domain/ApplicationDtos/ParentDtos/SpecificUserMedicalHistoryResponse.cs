using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class SpecificUserMedicalHistoryResponse
    {
        public int UserId { get; set; }
        public int MedicalRecordId { get; set; }
        public string DoctorName { get; set; } = null!;
        public DateTime GivenAt { get; set; }
        public string Description { get; set; } = null!;
        public string Medicine { get; set; } = null!;
        public List<string>? MedicalRecordImages { get; set; } = new List<string>();
    }
}
