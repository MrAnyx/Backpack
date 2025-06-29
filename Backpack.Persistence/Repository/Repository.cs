using Backpack.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence.Repository;

public abstract class Repository<TEntity>(ApplicationDbContext Context) where TEntity : Entity
{
    public Task<TEntity?> GetByIdAsync(uint id)
    {
        return Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }
}
