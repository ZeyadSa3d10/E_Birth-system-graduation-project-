using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class UserMedicalRecord
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string Medicine { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        
        #region User
        public Parent? Parent { get; set; }
        public int? ParentId { get; set; } =null!;
        public Child? child { get; set; }
        public int? ChildId { get; set; }=null!;
        #endregion

        #region Added By Doctor
        public Doctor Doctor { get; set; } = null!;
        public int DoctorId { get; set; } 
        #endregion

        public ICollection<MedicalRecordImage> MedicalRecordImages { get; set; } = new List<MedicalRecordImage>();
    }
}
