using Backpack.Domain.Configuration;
using Backpack.Presentation.Feature.Menu.About.Container;
using Backpack.Presentation.Model;
using Backpack.Presentation.Service;
using Backpack.Shared;
using Backpack.Shared.Helper;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Menu.About;

public partial class AboutDialogVM(AppSettings _settings) : DialogViewModel
{
    public IEnumerable<AboutItem> AboutItems { get; } = [
        new() { Name = TranslationManager.Translate("About_ApplicationName"), Description = Constant.ApplicationName },
        new() { Name = TranslationManager.Translate("About_Company"), Description = Constant.Company },
        new() { Name = TranslationManager.Translate("About_CurrentVersion"), Description = Constant.Version },
        new() { Name = TranslationManager.Translate("About_DotnetVersion"), Description = Constant.TargetDotnetVersion },
        new() { Name = TranslationManager.Translate("About_BuildDate"), Description = Constant.BuildDate },
        new() { Name = TranslationManager.Translate("About_Environment"), Description = _settings.Environment.ToString() },
        new() { Name = TranslationManager.Translate("About_Repository"), Description = Constant.Repository },
        new() { Name = TranslationManager.Translate("About_LogsPath"), Description = new Uri(PathResolver.GetLogsPath(_settings.Environment)) },
    ];

    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync();

    [RelayCommand]
    private void ExecuteOpenLink(Uri url)
    {
        if (!string.IsNullOrWhiteSpace(url.ToString()))
        {
            Process.Start(new ProcessStartInfo(url.ToString()) { UseShellExecute = true });
        }
    }
}
