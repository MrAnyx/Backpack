using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Entity;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Backup.Dialog;

public partial class AddOrUpdateBackupDialogVM(IWildcardRepository _wildcardRepository) : DialogViewModel
{
    public required Domain.Entity.Backup? Backup { get; init; }

    public override async Task OnActivatedAsync()
    {
        Name = Backup?.Name ?? string.Empty;
        Overwrite = Backup?.Overwrite ?? true;

        if (Backup != null)
        {
            var wildcards = await _wildcardRepository.GetAllAsync(q => q.Where(w => w.BackupId == Backup.Id));
            Wildcards.AddRange(wildcards);

        }
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
        Wildcards.Add(new()
        {
            BackupId = 0,
            Pattern = "Test.*"
        });
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool overwrite;

    public ObservableCollection<Wildcard> Wildcards { get; } = [];

    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync(false);

    [RelayCommand]
    private async Task ExecuteSave() => await CloseAsync(true);
}
