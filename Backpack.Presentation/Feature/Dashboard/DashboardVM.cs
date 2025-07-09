using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;

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

    public List<Backup> Backups { get; } = [];

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
        });

        WeakReferenceMessenger.Default.Register<NewFailedBackupMessage>(this, (r, m) =>
        {
            TotalBackups++;
            TotalFailedBackups++;
        });

        return Task.CompletedTask;
    }

    [RelayCommand]
    private void ExecuteCreateNewBackup()
    {
        WeakReferenceMessenger.Default.Send(new NewSuccessfulBackupMessage(new() { FileCount = 9, Size = 400, Status = Domain.Enum.eBackupStatus.Success, Trigger = Domain.Enum.eBackupTrigger.Manual, Type = Domain.Enum.eBackupType.Full }));
    }
}
