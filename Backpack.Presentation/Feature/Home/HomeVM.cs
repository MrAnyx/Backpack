using Backpack.Presentation.Model;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Home;

public partial class HomeVM : FeatureViewModel
{
    public override string Name => "Home";
    public override PackIconKind Icon => PackIconKind.House;
    public override uint Priority => uint.MaxValue;
}
