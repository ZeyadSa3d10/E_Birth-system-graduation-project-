using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.AuthDtos
{
    public class UserLogin
    {
        public string EmailOrNationalId { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
