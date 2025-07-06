namespace Backpack.Domain.Contract;
public interface IRepository<TEntity> where TEntity : Model.Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(uint id);
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    Task<TEntity> RemoveByIdAsync(uint id);
    Task<int> CountAllAsync();
}
