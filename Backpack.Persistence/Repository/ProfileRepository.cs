using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;

namespace Backpack.Persistence.Repository;

public class ProfileRepository(ApplicationDbContext Context) : Repository<Profile>(Context), IProfileRepository
{
}
