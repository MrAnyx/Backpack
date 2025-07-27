using Backpack.Presentation.Helper;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Backup.Dialog;

public partial class AddOrUpdateBackupDialogVM() : DialogViewModel
{
    public partial class ObservableAddOrUpdateBackupDTO : ObservableAdapter<Domain.Entity.Backup, ObservableAddOrUpdateBackupDTO>
    {
        [ObservableProperty]
        [Domain.Attribute.NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
        private string name = string.Empty;

        [ObservableProperty]
        private bool overwrite;

        [ObservableProperty]
        private string ignores = string.Empty;

        [ObservableProperty]
        [Domain.Attribute.NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
        private string sourcePath = string.Empty;

        [ObservableProperty]
        [Domain.Attribute.NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
        private string destinationPath = string.Empty;
    }

    public required Domain.Entity.Backup? Backup { get; set; }

    public ObservableAddOrUpdateBackupDTO ObservableBackup { get; private set; } = default!;

    public override Task OnActivatedAsync()
    {
        ObservableBackup = new();

        if (Backup != null)
        {
            ObservableBackup.FromEntity(Backup);
        }

        BindingHelper.BindTriggerCanExecute(ObservableBackup, this);

        return Task.CompletedTask;
    }


    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync(false);

    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task ExecuteSave() => await CloseAsync(true);
    private bool CanExecuteSave() => !string.IsNullOrWhiteSpace(ObservableBackup.Name) && !string.IsNullOrWhiteSpace(ObservableBackup.SourcePath) && !string.IsNullOrWhiteSpace(ObservableBackup.DestinationPath);

    [RelayCommand]
    private void SelectSourcePath()
    {
        if (FileExplorerHelper.TryExploreFile(out var path))
        {
            ObservableBackup.SourcePath = path;
        }
    }

    [RelayCommand]
    private void SelectDestinationPath()
    {
        if (FileExplorerHelper.TryExploreFile(out var path))
        {
            ObservableBackup.DestinationPath = path;
        }
    }
}
