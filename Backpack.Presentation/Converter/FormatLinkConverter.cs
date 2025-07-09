using System.Globalization;
using System.Windows.Data;

namespace Backpack.Presentation.Converter;

public class FormatLinkConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Uri uri)
        {
            if (uri.IsFile)
            {
                return uri.LocalPath; // C:\path\to\file.txt
            }

            return uri.ToString(); // keep http/https as-is
        }
        else if (value is string s)
        {
            if (Uri.TryCreate(s, UriKind.Absolute, out var parsedUri))
            {
                if (parsedUri.IsFile)
                {
                    return parsedUri.LocalPath;
                }

                return parsedUri.ToString(); // http(s)
            }

            return s; // fallback
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
