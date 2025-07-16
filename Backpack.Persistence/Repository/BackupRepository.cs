using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;

namespace Backpack.Persistence.Repository;

public class BackupRepository(ApplicationDbContext _context) : Repository<Backup>(_context), IBackupRepository
{
}
