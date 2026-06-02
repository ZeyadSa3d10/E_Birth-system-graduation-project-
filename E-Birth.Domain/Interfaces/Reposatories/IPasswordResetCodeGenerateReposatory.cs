using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IPasswordResetCodeGenerateReposatory :IGenericReposatory<OtpCode>
    {
        public Task<OtpCode> GetLastCodeByEmail(string email, CancellationToken cancellationToken);
    }
}
