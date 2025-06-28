using Backpack.Domain.Contract;

namespace Backpack.Persistence;

public class UnitOfWork(ApplicationDbContext _context) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
