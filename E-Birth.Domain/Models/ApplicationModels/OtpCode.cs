using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Models.ApplicationModels
{
    public class OtpCode
    {
        public int Id { get; set; }
        public string HashCode { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
