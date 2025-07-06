using Backpack.Domain.Contract;
using Backpack.Domain.Entity;

namespace Backpack.Persistence.Repository;

public class BackupRepository(ApplicationDbContext Context) : Repository<Backup>(Context), IBackupRepository
{
}
