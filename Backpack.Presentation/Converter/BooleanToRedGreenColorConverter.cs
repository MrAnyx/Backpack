using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Backpack.Presentation.Converter;

public class BooleanToRedGreenColorConverter : IValueConverter
{
    private static SolidColorBrush DangerColor => (System.Windows.Application.Current.Resources["Danger"] as SolidColorBrush)!;
    private static SolidColorBrush SuccessColor => (System.Windows.Application.Current.Resources["Success"] as SolidColorBrush)!;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? SuccessColor : DangerColor;
        }

        return DangerColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
