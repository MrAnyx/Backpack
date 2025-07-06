using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

class DateTimeToLocalTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime date)
        {
            return value;
        }

        string output = date.ToLocalTime().ToString("F", culture);
        return output;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
