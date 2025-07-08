using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Backpack.Domain.Enum;
using Backpack.Presentation.Feature.Dashboard;
using Backpack.Presentation.Feature.Menu.About;
using Backpack.Presentation.Model;
using Backpack.Shared.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM(
    IServiceProvider _provider,
    AppSettings _settings,
    ISnackbarMessageQueue _snackbar,
    IStatusBarMessageService _statusBar
) : ViewModel
{
    [ObservableProperty]
    private FeatureViewModel currentPage = default!;

    [ObservableProperty]
    private bool isLoaded = false;

    public IEnumerable<FeatureViewModel> Pages { get; } = _provider.GetServices<FeatureViewModel>().OrderByDescending(p => p.Priority).ThenBy(p => p.Name);
    public eAppEnvironment ApplicationEnvironment { get; } = _settings.Environment;
    public ISnackbarMessageQueue Snackbar { get; } = _snackbar;
    public IStatusBarMessageService StatusBar { get; } = _statusBar;

    [RelayCommand]
    private async Task ExecuteLoaded()
    {
        CurrentPage = Pages.First(s => s is DashboardVM);

        // Execute OnStartup on all ViewModel
        await Task.WhenAll(Pages.Select(vm => vm.OnStartupAsync()));

        await CurrentPage.LoadAsync();

        IsLoaded = true;
    }

    [RelayCommand]
    private async Task ExecuteNavigateTo(FeatureViewModel viewModel)
    {
        await CurrentPage.UnloadAsync();
        CurrentPage = viewModel;
        await CurrentPage.LoadAsync();
    }

    #region Menu commands
    [RelayCommand]
    private void ExecuteNewWindow()
    {
        var appPath = Environment.ProcessPath;
        if (appPath != null)
        {
            Process.Start(appPath);
            _snackbar.Enqueue("New window opened");
        }
    }

    [RelayCommand]
    private void ExecuteShowLog()
    {
        var logFile = PathResolver.GetLogFilePath(_settings.Environment);
        FileHelper.OpenFile(logFile);
    }

    [RelayCommand]
    private void ExecuteCloseWindow()
    {
        System.Windows.Application.Current.Shutdown();
    }

    [RelayCommand]
    private async Task ExecuteShowAbout()
    {
        var aboutDialogVM = _provider.GetRequiredService<AboutDialogVM>();
        await aboutDialogVM.ShowAsync(eDialogIdentifier.Core);
    }
    #endregion
}
