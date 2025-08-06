using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace Backpack.Shared;

public static class Constant
{
    public const string ApplicationName = "Backpack";

    public static IEnumerable<CultureInfo> AvailableCultures = [
        new CultureInfo("en"),
        new CultureInfo("fr"),
    ];
    public static CultureInfo DefaultCulture = new(AvailableCultures.Any(c => c.TwoLetterISOLanguageName == CultureInfo.CurrentCulture.TwoLetterISOLanguageName) ? CultureInfo.CurrentCulture.TwoLetterISOLanguageName : "en");

    public const string LogFileName = "app.log";
    public const string DatabaseFileName = "context.db";
    public const string RememberMeFileName = ".credentials";
    public const string NLogInternalLogFileName = "nlog.log";
    public const string UserPreferencesFileName = "preferences.json";

    public static string Company = Assembly.GetExecutingAssembly()!
            .GetCustomAttribute<AssemblyCompanyAttribute>()!
            .Company;

    public static string Version = Assembly.GetExecutingAssembly()!
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

    public static string TargetDotnetVersion = Assembly.GetEntryAssembly()!
            .GetCustomAttribute<TargetFrameworkAttribute>()!
            .FrameworkName;

    public static string CurrentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

    public static string ComputerName = Environment.MachineName;

    public static DateTime BuildDate = DateTime.ParseExact(Assembly.GetExecutingAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "BuildDate")!.Value!, "u", CultureInfo.InvariantCulture);

    public static Uri Repository = new("https://github.com/MrAnyx/Backpack");
}
