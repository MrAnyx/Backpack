using Backpack.Domain.Contract.Persistence;
using Backpack.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence.Repository;

public class UnitOfWork(ApplicationDbContext _context) : IUnitOfWork
{
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new DatabaseTransaction(transaction);
    }

    public int SaveChanges()
    {
        UpdateTimestamps();
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return _context.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = _context.ChangeTracker
            .Entries<IHasTimestamps>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = entry.Entity;

            var now = DateTime.Now;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }

            entity.UpdatedAt = now;
        }
    }
}
