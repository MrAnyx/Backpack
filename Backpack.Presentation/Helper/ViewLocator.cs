using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Backpack.Presentation.Helper;

public class ViewLocator : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item == null)
        {
            return null;
        }

        var viewModelType = item.GetType();
        var viewTypeName = Regex.Replace(viewModelType.FullName!, @"VM$", "", RegexOptions.IgnoreCase);
        var viewType = Type.GetType(viewTypeName) ?? throw new InvalidOperationException($"View not found for {viewModelType.FullName}");

        var dataTemplate = new DataTemplate
        {
            DataType = viewModelType,
            VisualTree = new FrameworkElementFactory(viewType)
        };

        return dataTemplate;
    }
}
