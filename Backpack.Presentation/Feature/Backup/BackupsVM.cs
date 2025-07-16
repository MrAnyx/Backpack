using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Backup;

public partial class BackupsVM(
    IBackupRepository _backupRepository
) : FeatureViewModel
{
    public override string Name => "Backups";
    public override PackIconKind Icon => PackIconKind.Backup;

    public override uint Priority => base.Priority;

    public ObservableCollection<Domain.Entity.Backup> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        var backups = await _backupRepository.GetAllAsync();
        Backups.AddRange(backups);
    }

    public override Task OnActivatedAsync()
    {
        return base.OnActivatedAsync();
    }

    public override Task OnDeactivatedAsync()
    {
        return base.OnDeactivatedAsync();
    }

    public override Task DisposeAsync()
    {
        return base.DisposeAsync();
    }

    [RelayCommand]
    private async Task CreateNewBackup()
    {
        var addOrUpdateBackupDialogVM = new AddOrUpdateBackupDialogVM();
        await addOrUpdateBackupDialogVM.ShowAsync(eDialogIdentifier.Core);
    }
}
