using System;
using System.Collections.Generic;
using System.Text;

namespace E_Birth.Application.Configratin
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int ExpiresInMinites { get; set; }
    }
}
