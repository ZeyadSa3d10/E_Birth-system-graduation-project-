using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class MedicalRecordImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        #region UserMedicalRecord
        public UserMedicalRecord UserMedicalRecord { get; set; } = null!;
        public int UserMedicalRecordId { get; set; }
        #endregion

    }
}
