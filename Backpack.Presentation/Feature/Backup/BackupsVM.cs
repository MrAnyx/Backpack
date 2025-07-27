using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Dialog.Confirm;
using Backpack.Presentation.Feature.Backup.Container;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Backup;

public partial class BackupsVM(
    IBackupRepository _backupRepository,
    IUnitOfWork _unitOfWork,
    IServiceProvider _provider,
    ISnackbarMessageQueue _snackbar
) : FeatureViewModel
{
    public override string Name => "Backups";
    public override PackIconKind Icon => PackIconKind.Backup;

    public override uint Priority => base.Priority;

    public FilterableObservableCollection<BackupTableItem> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        var backups = await _backupRepository.GetAllAsync();
        Backups.AddRange(backups.Select(b => new BackupTableItem(b)));
    }

    [RelayCommand]
    private async Task CreateNewBackup()
    {
        var viewModel = _provider.GetRequiredService<AddOrUpdateBackupDialogVM>();
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        var newBackup = _backupRepository.Add(viewModel.ObservableBackup.ToEntity());
        await _unitOfWork.SaveChangesAsync();
        Backups.Add(new BackupTableItem(newBackup));

        _snackbar.Enqueue($"Backup \"{newBackup.Name}\" created");
    }

    [RelayCommand]
    private async Task ExecuteEditRow(BackupTableItem backup)
    {
        var viewModel = _provider.GetRequiredService<AddOrUpdateBackupDialogVM>();
        viewModel.Backup = backup.Item;
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        backup.Item = viewModel.ObservableBackup.ToEntity();

        var updatedBackup = _backupRepository.Update(backup.Item);
        await _unitOfWork.SaveChangesAsync();

        Backups.UpdateBy(b => b.Item.Id == backup.Item.Id, b => b.Item = updatedBackup);

        _snackbar.Enqueue($"Backup \"{updatedBackup.Name}\" updated");
    }

    [RelayCommand]
    private async Task ExecuteDeleteRow(BackupTableItem backup)
    {
        var viewModel = _provider.GetRequiredService<ConfirmDialogVM>();
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        await _backupRepository.RemoveByIdAsync(backup.Item.Id);
        await _unitOfWork.SaveChangesAsync();
        Backups.Remove(backup);

        _snackbar.Enqueue($"Backup \"{backup.Item.Name}\" deleted");
    }
}
