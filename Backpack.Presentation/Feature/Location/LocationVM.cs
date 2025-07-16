using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Enum;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Location.Dialog;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Location;

public partial class LocationVM(
    ILocationRepository _locationRepository
) : FeatureViewModel
{
    public override string Name => "Locations";
    public override PackIconKind Icon => PackIconKind.Location;
    public override uint Priority => base.Priority;

    public ObservableCollection<Domain.Entity.Location> Locations { get; } = [];

    public override async Task OnStartupAsync()
    {
        var locations = await _locationRepository.GetAllAsync();
        Locations.AddRange(locations);
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
    private async Task ExecuteCreateNewLocation()
    {
        var dialogVM = new AddOrUpdateFileLocationDialogVM();
        await dialogVM.ShowAsync(eDialogIdentifier.Core);
    }
}
