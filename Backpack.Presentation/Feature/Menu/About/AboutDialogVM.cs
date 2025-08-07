using Backpack.Domain.Configuration;
using Backpack.Presentation.Feature.Menu.About.Container;
using Backpack.Presentation.Model;
using Backpack.Shared;
using Backpack.Shared.Helper;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backpack.Presentation.Feature.Menu.About;

public partial class AboutDialogVM(
    AppSettings _settings
) : DialogViewModel
{
    public IEnumerable<AboutItem> AboutItems { get; } = [
        new() { Name = "Application name", Description = Constant.ApplicationName },
        new() { Name = "Company", Description = Constant.Company },
        new() { Name = "Current version", Description = Constant.Version },
        new() { Name = ".NET Version", Description = Constant.TargetDotnetVersion },
        new() { Name = "Build date", Description = Constant.BuildDate },
        new() { Name = "Environment", Description = _settings.Environment.ToString() },
        new() { Name = "Github repository", Description = Constant.Repository },
        new() { Name = "Logs Path", Description = new Uri(PathResolver.GetLogsPath(_settings.Environment)) },
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
