using Backpack.Domain.Enum;
using Backpack.Persistence.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backpack.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder optionsBuilder = new();

        return new ApplicationDbContext(optionsBuilder.Configure(eAppEnvironment.Debug).Options);
    }
}

