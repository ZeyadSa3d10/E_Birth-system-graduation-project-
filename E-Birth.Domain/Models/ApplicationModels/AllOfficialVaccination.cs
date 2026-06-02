using E_Birth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class AllOfficialVaccination
    {
        // انا عامل ده علشان عندنا في الانيم هنخزن كل التطعيمات 
        //لاكن هنا هنخزن كل تطعيم والمعاد بتاع كل تطعيم المفروض يتاخد امتي 
        public int Id { get; set; }
        public string VaccinationType { get; set; }
        public int AgeInMonths { get; set; } // المعاد اللي المفروض يتاخد فيه التطعيم بالشهور
        public ICollection<ChildVaccination> ChildVaccinations { get; set; } = new List<ChildVaccination>();
    }
}
