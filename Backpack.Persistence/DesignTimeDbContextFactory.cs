using Backpack.Persistence.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backpack.Persistence;
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        return new ApplicationDbContext(optionsBuilder.Configure(Domain.Enum.eAppConfiguration.Debug).Options);
    }
}
