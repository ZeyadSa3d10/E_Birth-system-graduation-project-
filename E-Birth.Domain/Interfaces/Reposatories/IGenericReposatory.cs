using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Reposatories
{
    public interface IGenericReposatory<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> criteria,CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task<T> Update(T entity, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken);
    }
}
