using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;

namespace Backpack.Persistence.Repository;

public class LocationRepository(ApplicationDbContext Context) : Repository<Location>(Context), ILocationRepository
{
}
