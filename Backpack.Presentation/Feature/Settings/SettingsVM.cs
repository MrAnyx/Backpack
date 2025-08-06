using Backpack.Domain.Contract;
using Backpack.Presentation.Model;
using Backpack.Presentation.Service;
using Backpack.Shared;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Backpack.Presentation.Feature.Settings;

public partial class SettingsVM(
    IUserPreference _preferences
) : FeatureViewModel
{
    public override string Name => TranslationManager.Translate("Navigation_Settings");
    public override PackIconKind Icon => PackIconKind.Cog;

    public IEnumerable<CultureInfo> Languages { get; } = Constant.AvailableCultures;

    public CultureInfo Language
    {
        get => _preferences.Default.Culture;
        set
        {
            if (SetProperty(_preferences.Default.Culture, value, _preferences.Default, (x, y) => x.Culture = y))
            {
                MessageBox.Show("You need to restart the application to fully apply the new settings.", "Settings changed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
