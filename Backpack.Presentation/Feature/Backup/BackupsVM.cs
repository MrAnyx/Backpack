using Backpack.Application.UseCase.Backup;
using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Enum;
using Backpack.Presentation.Dialog.Confirm;
using Backpack.Presentation.Feature.Backup.Container;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Helper;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using Backpack.Presentation.Service;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Backup;

public partial class BackupsVM(
    IServiceProvider _provider,
    ISnackbarMessageQueue _snackbar,
    IMediator _mediator
) : FeatureViewModel
{
    public override string Name => TranslationManager.Translate("Navigation_Backups");
    public override PackIconKind Icon => PackIconKind.Backup;
    public override uint Order => 1;

    public FilterableObservableCollection<BackupTableItem> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        var backups = await _mediator.QueryAsync(new GetAllBackupsQuery());
        Backups.AddRange(backups.Value.Select(b => new BackupTableItem(b)));
    }

    public override Task OnActivatedAsync()
    {
        WeakReferenceMessenger.Default.Register<NewBackupMessage>(this, (r, m) => Backups.Add(new BackupTableItem(m.Value)));
        WeakReferenceMessenger.Default.Register<UpdateBackupMessage>(this, (r, m) => Backups.UpdateBy(b => b.Item.Id == m.Value.Id, b => b.Item = m.Value));
        WeakReferenceMessenger.Default.Register<DeleteBackupMessage>(this, (r, m) => Backups.RemoveBy((b) => b.Item.Id == m.Value.Id));

        return Task.CompletedTask;
    }

    public override Task OnDeactivatedAsync()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task CreateNewBackup()
    {
        var viewModel = new AddOrUpdateBackupDialogVM();
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        var newBackup = await _mediator.SendAsync(new NewBackupCommand() { Backup = viewModel.Backup });
        WeakReferenceMessenger.Default.Send(new NewBackupMessage(newBackup.Value));

        _snackbar.Enqueue(TranslationManager.Translate("Backup_Snackbar_NewBackupCreated", newBackup.Value.Name));
    }

    [RelayCommand]
    private async Task ExecuteEditRow(BackupTableItem backup)
    {
        var viewModel = new AddOrUpdateBackupDialogVM(backup.Item);
        var confirmation = await viewModel.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!confirmation)
        {
            return;
        }

        var updatedBackup = await _mediator.SendAsync(new UpdateBackupCommand() { Backup = viewModel.Backup });
        WeakReferenceMessenger.Default.Send(new UpdateBackupMessage(viewModel.Backup));

        _snackbar.Enqueue($"Backup \"{updatedBackup.Value.Name}\" updated");
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

        await _mediator.SendAsync(new DeleteBackupCommand() { Backup = backup.Item });
        WeakReferenceMessenger.Default.Send(new DeleteBackupMessage(backup.Item));

        _snackbar.Enqueue($"Backup \"{backup.Item.Name}\" deleted");
    }

    [RelayCommand]
    private void ExecuteOpenPathLocation(string path)
    {
        try
        {
            FileExplorerHelper.OpenFileExplorer(path);
        }
        catch (FileNotFoundException)
        {
            _snackbar.Enqueue($"Path \"{path}\" doesn't exist");
        }
    }
}
