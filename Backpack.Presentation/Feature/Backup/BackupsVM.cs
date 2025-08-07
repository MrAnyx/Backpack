using Backpack.Application.UseCase.Backup;
using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Enum;
using Backpack.Presentation.Dialog.Confirm;
using Backpack.Presentation.Feature.Backup.Container;
using Backpack.Presentation.Feature.Backup.Dialog;
using Backpack.Presentation.Helper;
using Backpack.Presentation.Message;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public override string Name => "Backups";
    public override PackIconKind Icon => PackIconKind.Backup;
    public override uint Order => 1;

    [ObservableProperty]
    private bool? selectAll = false;

    [ObservableProperty]
    private string search = string.Empty;
    partial void OnSearchChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Backups.ClearFilter();
        }
        else
        {
            Backups.SetFilter(x => x.Item.Name.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        ExecuteUpdateSelectedRangeHeader();
    }

    public FilterableObservableCollection<BackupTableItem> Backups { get; } = [];

    public override async Task OnStartupAsync()
    {
        WeakReferenceMessenger.Default.Register<NewBackupMessage>(this, (r, m) => Backups.Add(new BackupTableItem(m.Value)));
        WeakReferenceMessenger.Default.Register<UpdateBackupMessage>(this, (r, m) => Backups.UpdateBy(b => b.Item.Id == m.Value.Id, b => b.Item = m.Value));
        WeakReferenceMessenger.Default.Register<DeleteBackupMessage>(this, (r, m) => Backups.RemoveBy((b) => m.Value.Any(x => x.Id == b.Item.Id)));

        var backups = await _mediator.QueryAsync(new GetAllBackupsQuery());
        Backups.AddRange(backups.Value.Select(b => new BackupTableItem(b)));
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

        _snackbar.Enqueue("New backup created");
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

        await _mediator.SendAsync(new DeleteBackupCommand() { Backups = [backup.Item] });
        WeakReferenceMessenger.Default.Send(new DeleteBackupMessage([backup.Item]));

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

    [RelayCommand]
    private void ExecuteTriggerSelectAll()
    {
        foreach (var item in Backups)
        {
            item.IsChecked = SelectAll ?? false;
        }

        DeleteSelectedBackupsCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void ExecuteUpdateSelectedRangeHeader()
    {
        if (Backups.Count > 0 && Backups.All(s => s.IsChecked))
        {
            SelectAll = true;
        }
        else if (Backups.Any(s => s.IsChecked))
        {
            SelectAll = null;
        }
        else
        {
            SelectAll = false;
        }

        DeleteSelectedBackupsCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanExecuteDeleteSelectedRows))]
    private async Task DeleteSelectedBackups()
    {
        var items = Backups.Where(s => s.IsChecked).Select(b => b.Item).ToList();

        if (items.Count == 0)
        {
            _snackbar.Enqueue($"You must select at least one element from the list");
            return;
        }

        var confirmationVM = new ConfirmDialogVM();
        var result = await confirmationVM.ShowAsync<bool>(eDialogIdentifier.Core);

        if (!result)
        {
            return;
        }

        await _mediator.SendAsync(new DeleteBackupCommand() { Backups = items });
        WeakReferenceMessenger.Default.Send(new DeleteBackupMessage(items));

        ExecuteUpdateSelectedRangeHeader();

        _snackbar.Enqueue($"{items.Count} Server(s) have been removed");
    }

    private bool CanExecuteDeleteSelectedRows() => Backups.Any(s => s.IsChecked);

}
