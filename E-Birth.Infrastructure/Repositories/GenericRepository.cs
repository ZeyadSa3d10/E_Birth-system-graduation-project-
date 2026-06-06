using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Birth.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericReposatory<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    public GenericRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _db.Set<T>().AddAsync(entity);
        return entity;
    }
    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Set<T>().CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Set<T>().CountAsync(criteria);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Set<T>().Where(criteria).ToListAsync(cancellationToken);
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Set<T>().FindAsync(id);
    }
    public async Task<T> Update(T entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _db.Set<T>().Update(entity);
        return entity;
    }

}
