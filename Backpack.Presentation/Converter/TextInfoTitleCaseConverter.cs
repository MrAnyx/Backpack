using System;
using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

internal class TextInfoTitleCaseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string text)
        {
            throw new ArgumentException($"Parameter {nameof(value)} is not a valid {nameof(String)}.");
        }

        return culture.TextInfo.ToTitleCase(text);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null!;
    }
}
