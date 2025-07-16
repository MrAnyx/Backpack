using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;

namespace Backpack.Persistence.Repository;

public class WildcardRepository(ApplicationDbContext _context) : Repository<Wildcard>(_context), IWildcardRepository
{
}
