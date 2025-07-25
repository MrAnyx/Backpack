using Backpack.Domain.Enum;
using MaterialDesignThemes.Wpf;
using System;
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
            eStatusBarMessageType.Success => PackIconKind.SuccessCircle,
            eStatusBarMessageType.Info => PackIconKind.InfoCircle,
            eStatusBarMessageType.Loading => PackIconKind.Clock,
            eStatusBarMessageType.Warning => PackIconKind.AlertBox,
            eStatusBarMessageType.Error => PackIconKind.AlertBox,
            _ => throw new NotSupportedException($"Status bar message type of type {type} is not supported")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
