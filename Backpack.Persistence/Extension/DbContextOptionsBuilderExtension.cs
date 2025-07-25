using Backpack.Domain.Enum;
using Backpack.Shared.Helper;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Backpack.Persistence.Extension;

public static class DbContextOptionsBuilderExtension
{
    public static DbContextOptionsBuilder Configure(this DbContextOptionsBuilder options, eAppEnvironment environment = eAppEnvironment.Debug)
    {
        // Generate the database path
        var dbPath = PathResolver.GetDatabaseFilePath(environment);

        // Create the folder if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        options
            .UseSqlite($"Data Source={dbPath}")
            .UseLazyLoadingProxies()
        ;

        return options;
    }
}
