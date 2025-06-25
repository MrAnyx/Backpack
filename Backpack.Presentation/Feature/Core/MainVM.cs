using Backpack.Presentation.Feature.Home;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM : ObservableRecipient
{
    private readonly IServiceProvider _provider;

    [ObservableProperty]
    private ViewModel currentVM;

    public ObservableCollection<ViewModel> Pages { get; }

    public MainVM(IServiceProvider provider)
    {
        _provider = provider;

        var services = _provider.GetServices<ViewModel>();
        Pages = [.. services];
        CurrentVM = services.First(s => s is HomeVM);
    }

    [RelayCommand]
    private async Task Loaded()
    {
        // Execute OnStartup on all ViewModel
        await Task.WhenAll(_provider.GetServices<ViewModel>().Select(vm => vm.OnStartupAsync()));

        await CurrentVM.OnLoadedAsync();
    }
}
