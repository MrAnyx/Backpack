using Backpack.Presentation.Helper;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Backpack.Presentation.Feature.Backup.Dialog;

public partial class AddOrUpdateBackupDialogVM() : DialogViewModel
{
    public required Domain.Entity.Backup? Backup { get; set; }

    public override Task OnActivatedAsync()
    {
        Name = Backup?.Name ?? string.Empty;
        Overwrite = Backup?.Overwrite ?? true;
        Ignores = Backup?.Ignore ?? string.Empty;
        Source = Backup?.SourcePath ?? string.Empty;
        Destination = Backup?.DestinationPath ?? string.Empty;

        return Task.CompletedTask;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
    private string name = string.Empty;

    [ObservableProperty]
    private bool overwrite;

    [ObservableProperty]
    private string ignores = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
    private string source = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSaveCommand))]
    private string destination = string.Empty;

    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync(false);

    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task ExecuteSave() => await CloseAsync(true);
    private bool CanExecuteSave() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Source) && !string.IsNullOrWhiteSpace(Destination);

    [RelayCommand]
    private void SelectSourcePath()
    {
        if (FileExplorerHelper.TryExploreFile(out var path))
        {
            Source = path;
        }
    }

    [RelayCommand]
    private void SelectDestinationPath()
    {
        if (FileExplorerHelper.TryExploreFile(out var path))
        {
            Destination = path;
        }
    }
}
