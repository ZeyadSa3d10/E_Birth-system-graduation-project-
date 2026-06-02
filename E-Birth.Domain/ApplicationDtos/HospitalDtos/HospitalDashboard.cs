using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.HospitalDtos
{
    public class HospitalDashboard
    {
        public string HospitalName { get; set; } = null!;
        public int hospitalId { get; set; }
        public int TotalDoctors { get; set; }
        //public int AcceptedDoctors { get; set; }
        public int WaitingDoctors { get; set; }
        public int TotalParents { get; set; }
        public int TotalChilds { get; set; }
        public int ChildWithLateVaccinate { get; set; }
    }
}
