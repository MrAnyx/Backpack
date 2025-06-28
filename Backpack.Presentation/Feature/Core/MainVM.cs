using Backpack.Presentation.Feature.Home;
using Backpack.Presentation.Model;
using Backpack.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM(
    IServiceProvider _provider,
    ILogger<MainVM> _logger,
    ISnackbarMessageQueue _snackbar
) : ObservableRecipient
{
    [ObservableProperty]
    private FeatureViewModel currentPage = default!;

    [ObservableProperty]
    private FeatureViewModel selectedPage = default!;

    public IEnumerable<FeatureViewModel> Pages { get; } = _provider.GetServices<FeatureViewModel>().OrderByDescending(p => p.Priority).ThenBy(p => p.Name);
    public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
    public ISnackbarMessageQueue Snackbar { get; } = _snackbar;
    public string Title { get; } = Constant.ApplicationName;

    [RelayCommand]
    private async Task Loaded()
    {
        SelectedPage = CurrentPage = Pages.First(s => s is HomeVM);

        // Execute OnStartup on all ViewModel
        await Task.WhenAll(Pages.Select(vm => vm.OnStartupAsync()));

        await CurrentPage.LoadAsync();
    }

    [RelayCommand]
    private async Task NavigationChanged()
    {
        await CurrentPage.UnloadAsync();
        CurrentPage = SelectedPage;
        await CurrentPage.LoadAsync();
    }
}
