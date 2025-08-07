using Backpack.Domain.Contract;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Backpack.Infrastructure.Service;

public class TranslationManager : ITranslationManager
{
    private readonly ResourceManager _resourceManager = new("Backpack.Infrastructure.Localization.Language", typeof(TranslationManager).Assembly);

    public void ApplyCulture(CultureInfo culture)
    {
        // Set the culture for all threads (including default for new threads)
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        // Optionally set for the current thread too
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    public string Translate(string key, params object[]? args)
    {
        var culture = Thread.CurrentThread.CurrentUICulture;
        var raw = _resourceManager.GetString(key, culture);

        if (string.IsNullOrWhiteSpace(raw))
        {
            return $"!{key}!"; // fallback for missing keys
        }

        return args == null || args.Length == 0 ? raw : string.Format(culture, raw, args);
    }
}