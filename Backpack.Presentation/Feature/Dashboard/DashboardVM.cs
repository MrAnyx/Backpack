using Backpack.Domain.Contract.Repository;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
    IBackupRepository _backupRepository
) : FeatureViewModel
{
    public override string Name => "Dashboard";
    public override PackIconKind Icon => PackIconKind.ViewDashboard;
    public override uint Order => 0;

    [ObservableProperty]
    private int totalBackupLocations = 0;

    public ObservableCollection<Domain.Entity.History> History { get; } = [];

    public override async Task OnStartupAsync()
    {
        WeakReferenceMessenger.Default.Register<NewBackupMessage>(this, (r, m) => TotalBackupLocations++);
        WeakReferenceMessenger.Default.Register<DeleteBackupMessage>(this, (r, m) => TotalBackupLocations -= m.Value.Count());

        TotalBackupLocations = await _backupRepository.CountAllAsync();
    }
}
