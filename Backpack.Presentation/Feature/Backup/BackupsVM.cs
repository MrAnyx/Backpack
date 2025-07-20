using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Dialog.Confirm;
using Backpack.Presentation.Feature.Backup.Container;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;

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

    public FilterableObservableCollection<BackupTableItem> Backups { get; } = new([]);

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

        var newBackup = _backupRepository.Add(new()
        {
            Name = viewModel.Name,
            Overwrite = viewModel.Overwrite,
            Ignore = viewModel.Ignores,
            SourcePath = viewModel.Source,
            DestinationPath = viewModel.Destination,
        });
        await _unitOfWork.SaveChangesAsync();
        Backups.Add(new BackupTableItem(newBackup));

        WeakReferenceMessenger.Default.Send(new NewBackupLocationCreatedMessage(newBackup));

        _snackbar.Enqueue($"Backup \"{newBackup.Name}\" created");
    }

    [RelayCommand]
    private async Task ExecuteEditRow(Domain.Entity.Backup backup)
    {
        var viewModel = _provider.GetRequiredService<AddOrUpdateBackupDialogVM>();
        viewModel.Backup = backup;
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        backup.Name = viewModel.Name;
        backup.Overwrite = viewModel.Overwrite;
        backup.Ignore = viewModel.Ignores;
        backup.SourcePath = viewModel.Source;
        backup.DestinationPath = viewModel.Destination;

        var updatedBackup = _backupRepository.Update(backup);
        await _unitOfWork.SaveChangesAsync();

        Backups.UpdateItem(b => b.Item.Id == backup.Id, b => b.Item = updatedBackup);

        _snackbar.Enqueue($"Backup \"{updatedBackup.Name}\" updated");
    }

    [RelayCommand]
    private async Task ExecuteDeleteRow(Domain.Entity.Backup backup)
    {
        var viewModel = _provider.GetRequiredService<ConfirmDialogVM>();
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        await _backupRepository.RemoveByIdAsync(backup.Id);
        await _unitOfWork.SaveChangesAsync();
        Backups.Remove(backup);

        _snackbar.Enqueue($"Backup \"{backup.Name}\" deleted");
    }
}
