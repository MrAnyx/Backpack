using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Persistence.Repository;

public abstract class Repository<TEntity>(ApplicationDbContext Context) : IRepository<TEntity> where TEntity : Entity
{
    public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? specification = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (specification is not null)
        {
            query = specification(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(uint id, CancellationToken cancellationToken = default)
    {
        return Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public TEntity Add(TEntity entity)
    {
        var entry = Context.Set<TEntity>().Add(entity);
        return entry.Entity;
    }

    public TEntity Update(TEntity entity)
    {
        var entry = Context.Set<TEntity>().Update(entity);
        return entry.Entity;
    }

    public TEntity Remove(TEntity entity)
    {
        var entry = Context.Set<TEntity>().Remove(entity);
        return entry.Entity;
    }

    public async Task<TEntity> RemoveByIdAsync(uint id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken) ?? throw new NullReferenceException($"No {typeof(TEntity).Name} found with id {id}");
        var entry = Context.Set<TEntity>().Remove(entity);
        return entry.Entity;
    }

    public Task<int> CountAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? specification = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (specification is not null)
        {
            query = specification(query);
        }

        return query.CountAsync(cancellationToken);
    }
}
