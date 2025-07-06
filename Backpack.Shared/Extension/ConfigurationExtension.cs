using Backpack.Shared.Helper;
using Microsoft.Extensions.Configuration;

namespace Backpack.Shared.Extension;

public static class ConfigurationExtension
{
    public static T As<T>(this IConfiguration section, bool validate = true, bool safe = false) where T : class
    {
        ArgumentNullException.ThrowIfNull(section);

        var settings = section.Get<T>();

        if (settings == null)
        {
            if (!safe)
            {
                throw new InvalidOperationException($"Unable to convert the configuration section to type {typeof(T).Name}");
            }

            settings = Activator.CreateInstance<T>() ?? throw new InvalidOperationException($"Unable to convert the configuration section to type {typeof(T).Name}");
        }

        if (validate)
        {
            Validator.Validate(settings);
        }

        return settings;
    }
}
