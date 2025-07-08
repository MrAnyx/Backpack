using Backpack.Domain.Contract;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
    IBackupRepository _backupRepository,
    IStatusBarMessageService _statusBar
) : FeatureViewModel
{
    public override string Name => "Dashboard";
    public override PackIconKind Icon => PackIconKind.ViewDashboard;
    public override uint Priority => uint.MaxValue;

    [ObservableProperty]
    private int countBackups = 0;

    public override async Task OnStartupAsync()
    {
        CountBackups = await _backupRepository.CountAllAsync();
    }

    public override Task LoadAsync()
    {
        // TODO Fixer ça
        _statusBar.Post("Hello World", eStatusBarMessageType.Warning);
        return base.LoadAsync();
    }

    public override Task UnloadAsync()
    {
        return base.UnloadAsync();
    }

    [RelayCommand]
    private void ExecuteStatusBarMessage(eStatusBarMessageType type)
    {
        _statusBar.Post("Hello World", type);
    }
}
