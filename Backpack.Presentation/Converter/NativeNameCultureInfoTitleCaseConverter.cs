using System;
using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

internal class NativeNameCultureInfoTitleCaseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not CultureInfo ci)
        {
            throw new ArgumentException($"Expected {nameof(CultureInfo)}, but got {value?.GetType().Name ?? "null"}.");
        }

        return ci.TextInfo.ToTitleCase(ci.NativeName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null!;
    }
}
