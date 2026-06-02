using E_Birth.Domain.Models.ApplicationModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }= null!;
        public string? NationalId { get; set; }
        public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();

    }
}
