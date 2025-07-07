using Backpack.Domain.Enum;
using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

public class StatusBarMessageTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not eStatusBarMessageType type)
        {
            throw new ArgumentException("Invalid status bar message type", nameof(value));
        }

        return type switch
        {
            eStatusBarMessageType.Info => PackIconKind.InfoCircleOutline,
            eStatusBarMessageType.Warning => PackIconKind.WarningCircleOutline,
            eStatusBarMessageType.Loading => PackIconKind.Loading,
            eStatusBarMessageType.Success => PackIconKind.SuccessCircleOutline,
            eStatusBarMessageType.Error => PackIconKind.ErrorOutline,
            _ => throw new NotSupportedException($"Status bar message type of type {type} is not supported")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
