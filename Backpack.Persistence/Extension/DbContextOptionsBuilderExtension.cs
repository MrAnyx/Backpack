using Backpack.Domain.Enum;
using Backpack.Shared;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence.Extension;

public static class DbContextOptionsBuilderExtension
{
    public static DbContextOptionsBuilder Configure(this DbContextOptionsBuilder options, eAppConfiguration? configuration = eAppConfiguration.Debug)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, Constant.ApplicationName, $"data.{configuration}.db");

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        options
            .UseSqlite($"Data Source={dbPath}")
            .UseLazyLoadingProxies();

        return options;
    }
}
