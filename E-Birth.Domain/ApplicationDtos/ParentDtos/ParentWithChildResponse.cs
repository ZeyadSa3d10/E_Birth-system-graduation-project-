using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class ParentWithChildResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal AgeWithYears { get; set; }
        public decimal AgeWithMonths { get; set; }
        public string Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string role { get; set; }
    }
}
