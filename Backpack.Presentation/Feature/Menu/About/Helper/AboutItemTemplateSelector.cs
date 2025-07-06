using Backpack.Presentation.Feature.Menu.About.Container;
using System.Windows;
using System.Windows.Controls;

namespace Backpack.Presentation.Feature.Menu.About.Helper;

public class AboutItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? TextTemplate { get; set; }
    public DataTemplate? LinkTemplate { get; set; }
    public DataTemplate? BoolTemplate { get; set; }
    public DataTemplate? DateTimeTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is AboutItem aboutItem)
        {
            return aboutItem.Description switch
            {
                Uri => LinkTemplate,
                bool => BoolTemplate,
                DateTime => DateTimeTemplate,
                _ => TextTemplate
            };
        }

        return base.SelectTemplate(item, container);
    }
}
