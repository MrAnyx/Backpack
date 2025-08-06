using System.Diagnostics;
using System.IO;

namespace Backpack.Shared.Helper;

public static class FileHelper
{
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static void OpenFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                using Process process = new();
                process.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                process.Start();
            }
            else
            {
                throw new FileNotFoundException($"File {path} not found");
            }
        }
        catch
        {
            throw;
        }
    }
}