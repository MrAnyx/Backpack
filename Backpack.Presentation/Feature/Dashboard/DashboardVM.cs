using Backpack.Application.Consumer.Backup;
using Backpack.Domain.Contract;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Feature.Dashboard;

public partial class DashboardVM(
    IMediator _mediator,
    ISnackbarMessageQueue _snackbar
) : FeatureViewModel
{
    public override string Name => "Dashboard";
    public override PackIconKind Icon => PackIconKind.ViewDashboard;
    public override uint Priority => uint.MaxValue;

    [ObservableProperty]
    private int countBackups = 0;

    public override async Task OnStartupAsync()
    {
        var result = await _mediator.QueryAsync(new CountBackups());

        if (result.IsSuccess)
        {
            CountBackups = result.Value;
        }
    }

    public override Task LoadAsync()
    {
        return base.LoadAsync();
    }

    public override Task UnloadAsync()
    {
        return base.UnloadAsync();
    }
}
