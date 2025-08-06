using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Backpack.Domain.Model;
using Backpack.Infrastructure.Json;
using Backpack.Shared;
using Backpack.Shared.Helper;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backpack.Infrastructure.Service;

public class UserPreference(AppSettings _settings) : IUserPreference
{
    public UserPreferenceInstance Default { get; set; } = new()
    {
        Culture = Constant.DefaultCulture
    };

    private readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new CultureInfoJsonConverter() }
    };

    public async Task LoadAsync()
    {
        var path = PathResolver.GetUserPreferencesFilePath(_settings.Environment);

        if (!File.Exists(path))
        {
            return;
        }

        var json = await File.ReadAllTextAsync(path);
        var loaded = JsonSerializer.Deserialize<UserPreferenceInstance>(json, JsonOptions);

        if (loaded != null)
        {
            Default = loaded;
        }
    }

    public async Task SaveAsync()
    {
        var path = PathResolver.GetUserPreferencesFilePath(_settings.Environment);

        var dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir!);
        }

        var json = JsonSerializer.Serialize(Default, JsonOptions);
        await File.WriteAllTextAsync(path, json);
    }
}
