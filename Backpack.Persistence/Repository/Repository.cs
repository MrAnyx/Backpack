using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence.Repository;

public abstract class Repository<TEntity>(ApplicationDbContext Context) : IRepository<TEntity> where TEntity : Entity
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public Task<TEntity?> GetByIdAsync(uint id)
    {
        return Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task<TEntity> RemoveByIdAsync(uint id)
    {
        var entity = await GetByIdAsync(id) ?? throw new NullReferenceException($"No {typeof(TEntity).Name} found with id {id}");
        var entry = Context.Set<TEntity>().Remove(entity);
        return entry.Entity;
    }

    public Task<int> CountAllAsync()
    {
        return Context.Set<TEntity>().CountAsync();
    }
}
