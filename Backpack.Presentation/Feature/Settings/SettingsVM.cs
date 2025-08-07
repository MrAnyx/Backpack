using Backpack.Domain.Contract;
using Backpack.Presentation.Model;
using Backpack.Shared;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Backpack.Presentation.Feature.Settings;

public partial class SettingsVM(
    IUserPreference _preferences,
    ITranslationManager _translation
) : FeatureViewModel
{
    public override string Name => _translation.Translate("Navigation_Settings");
    public override PackIconKind Icon => PackIconKind.Cog;

    public IEnumerable<CultureInfo> Languages { get; } = Constant.AvailableCultures;

    public CultureInfo Language
    {
        get => _preferences.Default.Culture;
        set
        {
            if (SetProperty(_preferences.Default.Culture, value, _preferences.Default, (x, y) => x.Culture = y))
            {
                _translation.ApplyCulture(value);

                MessageBox.Show(
                    _translation.Translate("Settings_RestartToSave_Body"),
                    _translation.Translate("Settings_RestartToSave_Title"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
