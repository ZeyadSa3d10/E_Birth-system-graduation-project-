using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.ApplicationDtos.ParentDtos
{
    public class DeleteParenet
    {
        [Required(ErrorMessage ="NationalId Is Required")]
        public string NationalId { get; set; } = null!;
    }
}
