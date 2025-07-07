using Backpack.Domain.Enum;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Backpack.Presentation.Converter;

public class StatusBarMessageTypeToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not eStatusBarMessageType type)
        {
            throw new ArgumentException("Invalid status bar message type", nameof(value));
        }

        return type switch
        {
            eStatusBarMessageType.Error => Brushes.DarkRed,
            eStatusBarMessageType.Warning => Brushes.Goldenrod,
            eStatusBarMessageType.Info => Brushes.CornflowerBlue,
            eStatusBarMessageType.Loading => Brushes.DimGray,
            eStatusBarMessageType.Success => Brushes.DarkGreen,
            _ => throw new NotSupportedException($"Status bar message type of type {type} is not supported")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
