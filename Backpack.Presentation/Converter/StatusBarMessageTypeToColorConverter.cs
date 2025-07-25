using Backpack.Domain.Enum;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Backpack.Presentation.Converter;

public class StatusBarMessageTypeToColorConverter : IValueConverter
{
    private static SolidColorBrush DangerColor => (System.Windows.Application.Current.Resources["Danger"] as SolidColorBrush)!;
    private static SolidColorBrush WarningColor => (System.Windows.Application.Current.Resources["Warning"] as SolidColorBrush)!;
    private static SolidColorBrush SuccessColor => (System.Windows.Application.Current.Resources["Success"] as SolidColorBrush)!;
    private static SolidColorBrush InfoColor => (System.Windows.Application.Current.Resources["Info"] as SolidColorBrush)!;
    private static SolidColorBrush GrayColor => (System.Windows.Application.Current.Resources["Gray"] as SolidColorBrush)!;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not eStatusBarMessageType type)
        {
            throw new ArgumentException("Invalid status bar message type", nameof(value));
        }

        return type switch
        {
            eStatusBarMessageType.Error => DangerColor,
            eStatusBarMessageType.Warning => WarningColor,
            eStatusBarMessageType.Info => InfoColor,
            eStatusBarMessageType.Loading => GrayColor,
            eStatusBarMessageType.Success => SuccessColor,
            _ => throw new NotSupportedException($"Status bar message type of type {type} is not supported")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
