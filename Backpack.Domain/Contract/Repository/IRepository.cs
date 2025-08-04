using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract.Repository;
public interface IRepository<TEntity> where TEntity : Model.Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? specification = null, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(uint id, CancellationToken cancellationToken = default);
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    Task<TEntity> RemoveByIdAsync(uint id, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? specification = null, CancellationToken cancellationToken = default);
}
