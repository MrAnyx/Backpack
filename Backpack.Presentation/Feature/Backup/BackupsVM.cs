using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Dialog.Confirm;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using Backpack.Shared.Extension;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

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
        Backups.Add(newBackup);

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

        backup.MergeWith(new()
        {
            Name = viewModel.Name,
            Overwrite = viewModel.Overwrite,
            Ignore = viewModel.Ignores,
            SourcePath = viewModel.Source,
            DestinationPath = viewModel.Destination,
        });

        var newBackup = _backupRepository.Update(backup);
        await _unitOfWork.SaveChangesAsync();

        Backups.ReplaceAll(b => b.Id == backup.Id, newBackup);

        _snackbar.Enqueue($"Backup \"{newBackup.Name}\" updated");
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
