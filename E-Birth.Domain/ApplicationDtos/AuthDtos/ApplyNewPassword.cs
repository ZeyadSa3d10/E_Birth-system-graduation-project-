using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.AuthDtos
{
    public class ApplyNewPassword
    {
        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain uppercase, lowercase, and number.")]
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Email { get; set; }
    }
}
