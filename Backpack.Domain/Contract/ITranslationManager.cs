using System.Globalization;

namespace Backpack.Domain.Contract;

public interface ITranslationManager
{
    void ApplyCulture(CultureInfo culture);
    string Translate(string key, params object[]? args);
}
