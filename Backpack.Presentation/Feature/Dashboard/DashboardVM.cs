using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
    IBackupRepository _backupRepository
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

    public ObservableCollection<Backup> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        TotalBackups = await _backupRepository.CountAllAsync();
        TotalSuccessfulBackups = await _backupRepository.CountAllAsync(q => q.Where(b => b.Status == Domain.Enum.eBackupStatus.Success));
        TotalFailedBackups = await _backupRepository.CountAllAsync(q => q.Where(b => b.Status == Domain.Enum.eBackupStatus.Error));

        var backups = await _backupRepository.GetAllAsync();
        Backups.AddRange(backups);
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
