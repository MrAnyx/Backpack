using Microsoft.EntityFrameworkCore.Storage;
using Backpack.Domain.Contract;

namespace Backpack.Persistence.Model;

public class DatabaseTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public DatabaseTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
        => _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default)
        => _transaction.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _transaction.DisposeAsync();
    }
}