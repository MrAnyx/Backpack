using System.Resources;
using System.Threading;
namespace Backpack.Presentation.Service;

public static class TranslationManager
{
    private static readonly ResourceManager _resourceManager = new("Backpack.Presentation.Resource.Localization.Language", typeof(TranslationManager).Assembly);

    public static string Translate(string key, params object[]? args)
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