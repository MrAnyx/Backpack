using Humanizer;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

public class HumanizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return value.ToString().Humanize();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
