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
using System.Diagnostics;
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

    public string LoadingMessage { get; } = LoadingMessages[new Random().Next(0, LoadingMessages.Length)];

    private static readonly string[] LoadingMessages = [
        "Still faster than your morning coffee...",
        "Loading... bribing the hamsters to run faster.",
        "Hold on, aligning the stars...",
        "Downloading data from the cloud (hope it's not raining).",

        "Chargement… plus rapide que les embouteillages de Paris !",
        "On met les baguettes en place…",
        "Patientez... on cherche encore la connexion Wi-Fi du voisin.",
        "Presque prêt… le fromage affine encore un peu.",

        "読み込み中… 忍者がデータをこっそり運んでいます。",
        "少々お待ちください…温泉から戻る途中です。",
        "データを召喚中…呪文がちょっと長いです。",
        "あとちょっと…猫が助けに来ています。",

        "加载中… 程序员在喝奶茶，请稍等。",
        "马上就好… 数据还在排队买早餐。",
        "加载中… 云端有点堵车。",
        "别急，程序正在翻墙回来。",

        "로딩 중… 치킨 먹는 중이라 조금만 기다려줘요!",
        "곧 시작합니다… 데이터가 지하철 타고 오는 중.",
        "잠시만요… 서버가 커피 타는 중입니다.",
        "로딩 중… 고양이가 도와주고 있어요.",

        "Cargando… aún más rápido que un lunes por la mañana.",
        "Un momento… el Wi-Fi está de siesta.",
        "Preparando todo… sin prisa, pero sin pausa.",
        "¡Casi listo! Los datos están calentando.",

        "Lade... noch schneller als die Deutsche Bahn!",
        "Moment mal... wir sortieren noch die Datenwürste.",
        "Ladevorgang läuft… Kaffee wird erst fertig.",
        "Fast da! Der Server zieht noch seine Socken an.",

        "Загрузка… медведь ещё не проснулся.",
        "Подождите… балалайка настраивается.",
        "Загружаем... почти как ракета, но с чайком.",
        "Секунду... матрёшки распаковываются."
    ];

    public IEnumerable<FeatureViewModel> Pages { get; } = _provider.GetServices<FeatureViewModel>().OrderByDescending(p => p.Priority).ThenBy(p => p.Name);
    public eAppEnvironment ApplicationEnvironment { get; } = _settings.Environment;
    public ISnackbarMessageQueue Snackbar { get; } = _snackbar;
    public IStatusBarMessageService StatusBar { get; } = _statusBar;

    public async Task LoadAsync()
    {
        // Database
        var pendingMigrations = await _migration.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            MessageBox.Show("The database structure has changed. Applying the latest version.", "Applying migrations", MessageBoxButton.OK, MessageBoxImage.Information);
            await _migration.MigrateAsync();
        }

        CurrentPage = Pages.First(s => s is DashboardVM);

        await Task.WhenAll(Pages.Select(vm => vm.OnStartupAsync()));
        await CurrentPage.OnActivatedAsync();
        CurrentPage.IsActive = true;

        IsActive = true;
        IsLoaded = true;
    }

    public async Task UnloadAsync()
    {
        await Task.WhenAll(Pages.Select(vm => vm.OnDeactivatedAsync()));
        await Task.WhenAll(Pages.Select(vm => vm.DisposeAsync()));
    }

    [RelayCommand]
    private async Task ExecuteNavigateTo(FeatureViewModel viewModel)
    {
        CurrentPage.IsActive = false;
        await CurrentPage.OnDeactivatedAsync();

        CurrentPage = viewModel;

        await CurrentPage.OnActivatedAsync();
        CurrentPage.IsActive = true;
    }

    #region Menu commands
    [RelayCommand]
    private void ExecuteNewWindow()
    {
        var appPath = Environment.ProcessPath;
        if (appPath != null)
        {
            _snackbar.Enqueue("Opening new window");
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
            MessageBox.Show("The database structure has changed. Applying the latest version.", "Applying migrations", MessageBoxButton.OK, MessageBoxImage.Information);
            await _migration.MigrateAsync();
        }
        else
        {
            MessageBox.Show("Your database is already up to date.", "Database update to date", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    #endregion
}
