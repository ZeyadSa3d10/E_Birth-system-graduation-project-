using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class ChildDetailsResponse
    {
        public int ChildId { get; set; }
        public string ChildFullName { get; set; } = null!;
        public string ChildNationalId { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Village { get; set; } = null!;
        public string City { get; set; } = null!;

        #region Gender
        public string Gender { get; set; } = null!;
        #endregion

        #region Governorate
        public string Governorate { get; set; } = null!;

        #endregion

        #region BloodType
        public string BloodType { get; set; } = null!;
        #endregion


        public string ParentFullName { get; set; } = null!;
        public string ParentPhoneNumber { get; set; } = null!;
        public string ParentNationalId { get; set; } = null!;
        public string ParentEmail { get; set; } = null!;
    }
}
