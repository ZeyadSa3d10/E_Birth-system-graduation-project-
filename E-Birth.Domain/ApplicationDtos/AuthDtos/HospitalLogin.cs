using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.AuthDtos
{
    public class HospitalLogin
    {
        public string Email { get; set; } = null!;
        public string Passworded { get; set; } = null!;
    }
}
