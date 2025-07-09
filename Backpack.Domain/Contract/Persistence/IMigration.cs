namespace Backpack.Domain.Contract.Persistence;
public interface IMigration
{
    Task<IEnumerable<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default);
    Task MigrateAsync(CancellationToken cancellationToken = default);
}
