using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract.Persistence;

public interface IUnitOfWork
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
