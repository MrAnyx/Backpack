using Backpack.Presentation.Model;
using Backpack.Presentation.Service;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Settings;
public partial class SettingsVM : FeatureViewModel
{
    public override string Name => TranslationManager.Translate("Navigation_Settings");
    public override PackIconKind Icon => PackIconKind.Cog;
}
