using Backpack.Presentation.Model;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Home;

public partial class HomeVM : ViewModel
{
    public override string Name => "Home";
    public override PackIconKind Icon => PackIconKind.House;
}
