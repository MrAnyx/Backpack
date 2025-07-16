using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
) : FeatureViewModel
{
    public override string Name => "Dashboard";
    public override PackIconKind Icon => PackIconKind.ViewDashboard;
    public override uint Priority => uint.MaxValue;

    [ObservableProperty]
    private int totalBackups = 0;

    [ObservableProperty]
    private int totalSuccessfulBackups = 0;

    [ObservableProperty]
    private int totalFailedBackups = 0;

    public ObservableCollection<Domain.Entity.Backup> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        TotalBackups = 0;
        TotalSuccessfulBackups = 0;
        TotalFailedBackups = 0;
    }

    public override Task OnActivatedAsync()
    {
        WeakReferenceMessenger.Default.Register<NewSuccessfulBackupMessage>(this, (r, m) =>
        {
            TotalBackups++;
            TotalSuccessfulBackups++;
            Backups.Add(m.Value);
        });

        WeakReferenceMessenger.Default.Register<NewFailedBackupMessage>(this, (r, m) =>
        {
            TotalBackups++;
            TotalFailedBackups++;
            Backups.Add(m.Value);
        });

        return Task.CompletedTask;
    }
}
