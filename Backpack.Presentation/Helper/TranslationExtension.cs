using Backpack.Domain.Contract;
using Backpack.Shared;
using Microsoft.Extensions.DependencyInjection;
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
        var _translation = Context.Services.GetRequiredService<ITranslationManager>();
        return _translation.Translate(Key, Parameters ?? []);
    }
}
