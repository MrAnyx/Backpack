using Backpack.Domain.Contract;
using Backpack.Domain.Contract.Repository;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
    IBackupRepository _backupRepository,
    ITranslationManager _translation
) : FeatureViewModel
{
    public override string Name => _translation.Translate("Navigation_Dashboard");
    public override PackIconKind Icon => PackIconKind.ViewDashboard;
    public override uint Order => 0;

    [ObservableProperty]
    private int totalBackupLocations = 0;

    public ObservableCollection<Domain.Entity.History> History { get; } = [];

    public override async Task OnActivatedAsync()
    {
        TotalBackupLocations = await _backupRepository.CountAllAsync();
    }
}
