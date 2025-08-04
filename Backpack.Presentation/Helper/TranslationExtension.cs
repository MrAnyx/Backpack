using Backpack.Presentation.Service;
using System;
using System.Windows.Markup;

namespace Backpack.Presentation.Helper;

[MarkupExtensionReturnType(typeof(string))]
public class TranslationExtension(string key) : MarkupExtension
{
    public string Key { get; set; } = key;
    public object[]? Parameters { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return TranslationManager.Translate(Key, Parameters ?? []);
    }
}
