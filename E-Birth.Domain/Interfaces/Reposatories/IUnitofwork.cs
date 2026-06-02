using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IUnitofwork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
