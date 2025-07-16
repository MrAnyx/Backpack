using Backpack.Presentation.Attribute;
using System.Reflection;
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

        // Try to get [View(typeof(...))] attribute
        var viewAttr = viewModelType.GetCustomAttribute<ViewAttribute>();
        var viewType = viewAttr?.ViewType;

        // Fallback to convention: remove "VM" suffix
        if (viewType == null)
        {
            var viewTypeName = Regex.Replace(viewModelType.FullName!, @"VM$", "", RegexOptions.IgnoreCase);
            viewType = Type.GetType(viewTypeName);
        }

        if (viewType == null)
        {
            throw new InvalidOperationException($"View not found for {viewModelType.FullName}");
        }

        var factory = new FrameworkElementFactory(viewType);

        return new DataTemplate
        {
            DataType = viewModelType,
            VisualTree = factory
        };
    }
}
