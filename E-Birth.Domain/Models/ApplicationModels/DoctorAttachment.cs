using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class DoctorAttachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public int DoctorId { get; set; }
    }
}
