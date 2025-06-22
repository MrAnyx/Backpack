using Backpack.Presentation.Model;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Settings;

public partial class SettingsVM : ViewModel
{
    public override string Name => "Settings";
    public override PackIconKind Icon => PackIconKind.Cog;
}
