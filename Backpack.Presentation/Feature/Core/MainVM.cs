using Backpack.Presentation.Feature.Home;
using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM : ObservableRecipient
{
    private readonly IServiceProvider _provider;

    [ObservableProperty]
    private FeatureViewModel currentPage;

    [ObservableProperty]
    private FeatureViewModel selectedPage;

    public IEnumerable<FeatureViewModel> Pages { get; }
    public string Username { get; }
    public string Domain { get; }
    public string Version { get; }

    public MainVM(IServiceProvider provider, ILogger<MainVM> _logger)
    {
        _logger.LogInformation("Test");

        _provider = provider;

        Domain = Environment.UserDomainName;
        Username = Environment.UserName;
        Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        Pages = _provider.GetServices<FeatureViewModel>().OrderByDescending(p => p.Priority).ThenBy(p => p.Name);
        SelectedPage = CurrentPage = Pages.First(s => s is HomeVM);
    }

    [RelayCommand]
    private async Task Loaded()
    {
        // Execute OnStartup on all ViewModel
        await Task.WhenAll(Pages.Select(vm => vm.OnStartupAsync()));

        await CurrentPage.OnLoadedAsync();
    }

    [RelayCommand]
    private async Task NavigationChanged()
    {
        await CurrentPage.OnUnloadedAsync();
        CurrentPage = SelectedPage;
        await CurrentPage.OnLoadedAsync();
    }
}
