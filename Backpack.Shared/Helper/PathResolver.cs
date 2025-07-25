using Backpack.Domain.Enum;
using System;
using System.IO;

namespace Backpack.Shared.Helper;

public static class PathResolver
{
    private static readonly string s_logPathSuffix = "Logs";
    private static readonly string s_logArchivePathSuffix = "Archives";

    public static string GetLocalApplicationDataPath(eAppEnvironment environment)
    {
        return Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Constant.ApplicationName,
            environment.ToString()
        );
    }

    public static string GetRoamingApplicationDataPath(eAppEnvironment environment)
    {
        return Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Constant.ApplicationName,
            environment.ToString()
        );
    }

    public static string GetLogsPath(eAppEnvironment environment)
    {
        return Path.Join(
            GetLocalApplicationDataPath(environment),
            s_logPathSuffix
        );
    }

    public static string GetLogFilePath(eAppEnvironment environment)
    {
        return Path.Join(
            GetLogsPath(environment),
            Constant.LogFileName
        );
    }

    public static string GetLogArchivesPath(eAppEnvironment environment)
    {
        return Path.Join(
            GetLogsPath(environment),
            s_logArchivePathSuffix
        );
    }

    public static string GetNLogFilePath(eAppEnvironment environment)
    {
        return Path.Join(
            GetLogsPath(environment),
            Constant.NLogInternalLogFileName
        );
    }

    public static string GetDatabaseFilePath(eAppEnvironment environment)
    {
        return Path.Join(
            GetRoamingApplicationDataPath(environment),
            Constant.DatabaseFileName
        );
    }

    public static string GetRememberMeFilePath(eAppEnvironment environment)
    {
        return Path.Join(
            GetRoamingApplicationDataPath(environment),
            Constant.RememberMeFileName
        );
    }
}
