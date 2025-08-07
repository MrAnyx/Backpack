using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Enum;
using Backpack.Presentation.Feature.Dashboard;
using Backpack.Presentation.Feature.Menu.About;
using Backpack.Presentation.Model;
using Backpack.Shared.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM(
    IServiceProvider _provider,
    AppSettings _settings,
    ISnackbarMessageQueue _snackbar,
    IStatusBarMessageService _statusBar,
    IMigration _migration
) : ViewModel
{
    [ObservableProperty]
    private FeatureViewModel currentPage = default!;

    [ObservableProperty]
    private bool isLoaded = false;

    private IEnumerable<string> LoadingMessages => [
            "Still faster than your morning coffee...",
            "Loading... bribing the hamsters to run faster.",
            "Hold on, aligning the stars...",
            "Downloading data from the cloud (hope it's not raining).",
    ];
    public string LoadingMessage { get; private set; } = string.Empty;

    public IEnumerable<FeatureViewModel> Pages { get; } = _provider.GetServices<FeatureViewModel>().OrderBy(p => p.Order).ThenBy(p => p.Name);
    public ISnackbarMessageQueue Snackbar { get; } = _snackbar;
    public IStatusBarMessageService StatusBar { get; } = _statusBar;

    public Task OnStartupAsync()
    {
        LoadingMessage = LoadingMessages.ElementAt(new Random().Next(0, LoadingMessages.Count()));
        return Task.CompletedTask;
    }

    public async Task OnActivatedAsync()
    {
        // Database
        var pendingMigrations = await _migration.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            MessageBox.Show(
                "The database structure has changed. Applying the latest version.",
                "Applying migrations",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            await _migration.MigrateAsync();
        }

        CurrentPage = Pages.First(s => s is DashboardVM);

        foreach (var page in Pages)
        {
            page.IsActive = true;
        }

        await Task.WhenAll(Pages.Select(vm => vm.OnStartupAsync()));
        await CurrentPage.OnActivatedAsync();

        IsActive = true;
        IsLoaded = true;
    }

    public async Task OnDeactivatedAsync()
    {
        await Task.WhenAll(Pages.Select(vm => vm.OnDeactivatedAsync()));
        await Task.WhenAll(Pages.Select(vm => vm.DisposeAsync()));

        foreach (var page in Pages)
        {
            page.IsActive = false;
        }
    }

    [RelayCommand]
    private async Task ExecuteNavigateTo(FeatureViewModel viewModel)
    {
        await CurrentPage.OnDeactivatedAsync();

        CurrentPage = viewModel;

        await CurrentPage.OnActivatedAsync();
    }

    #region Menu commands
    [RelayCommand]
    private void ExecuteNewWindow()
    {
        var appPath = Environment.ProcessPath;
        if (appPath != null)
        {
            Process.Start(appPath);
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

    [RelayCommand]
    private async Task ExecuteCheckDatabaseIntegrity()
    {
        var pendingMigrations = await _migration.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            MessageBox.Show(
                "The database structure has changed. Applying the latest version.",
                "Applying migrations",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            await _migration.MigrateAsync();
        }
        else
        {
            MessageBox.Show(
                "Your database is already up to date.",
                "Database up to date",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
    #endregion
}
