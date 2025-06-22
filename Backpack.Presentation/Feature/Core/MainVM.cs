using Backpack.Presentation.Feature.Settings;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM(IServiceProvider _services) : ObservableRecipient
{
    [ObservableProperty]
    private ViewModel currentVM = _services.GetRequiredService<SettingsVM>();

    public ObservableCollection<ViewModel> Pages = [.. _services.GetServices<ViewModel>()];

    [RelayCommand]
    private async Task Loaded()
    {
        // Execute OnStartup on all ViewModel
        await Task.WhenAll(_services.GetServices<ViewModel>().Select(vm => vm.OnStartupAsync()));

        await CurrentVM.OnLoadedAsync();
    }
}
