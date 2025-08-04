using Backpack.Presentation.Helper;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Backup.Dialog;

public partial class AddOrUpdateBackupDialogVM : DialogViewModel
{
    public AddOrUpdateBackupDialogVM(Domain.Entity.Backup? _initialBackup = null)
    {
        Backup = _initialBackup ?? new()
        {
            Name = string.Empty,
            Ignore = string.Empty,
            Overwrite = true,
            SourcePath = string.Empty,
            DestinationPath = string.Empty,
        };
    }

    public Domain.Entity.Backup Backup { get; }

    public string Name
    {
        get => Backup.Name;
        set
        {
            if (SetProperty(Backup.Name, value, Backup, (x, y) => x.Name = y))
            {
                ExecuteSaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public bool Overwrite
    {
        get => Backup.Overwrite;
        set => SetProperty(Backup.Overwrite, value, Backup, (x, y) => x.Overwrite = y);
    }

    public string Ignore
    {
        get => Backup.Ignore;
        set => SetProperty(Backup.Ignore, value, Backup, (x, y) => x.Ignore = y);
    }

    public string SourcePath
    {
        get => Backup.SourcePath;
        set
        {
            if (SetProperty(Backup.SourcePath, value, Backup, (x, y) => x.SourcePath = y))
            {
                ExecuteSaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string DestinationPath
    {
        get => Backup.DestinationPath;
        set
        {
            if (SetProperty(Backup.DestinationPath, value, Backup, (x, y) => x.DestinationPath = y))
            {
                ExecuteSaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync(false);

    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task ExecuteSave() => await CloseAsync(true);
    private bool CanExecuteSave() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);

    [RelayCommand]
    private void SelectSourcePath()
    {
        if (FileExplorerHelper.TrySelectFile(out var path))
        {
            SourcePath = path;
        }
    }

    [RelayCommand]
    private void SelectDestinationPath()
    {
        if (FileExplorerHelper.TrySelectFile(out var path))
        {
            DestinationPath = path;
        }
    }
}
