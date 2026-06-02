using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.AuthDtos
{
    public class SendOtp
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
