using Microsoft.Win32;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Backpack.Presentation.Helper;

public static class FileExplorerHelper
{
    public static bool TrySelectFile([NotNullWhen(true)] out string? path)
    {
        var openFileDialog = new OpenFolderDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            path = openFileDialog.FolderName;
            return true;
        }

        path = null;
        return false;
    }

    public static void OpenFileExplorer(string path, bool highlightSelection = false)
    {
        if (!Path.Exists(path))
        {
            throw new FileNotFoundException($"Path \"{path}\" doesn't exist.");
        }

        var arguments = highlightSelection ? $"/select,\"{path}\"" : $"\"{path}\"";

        ProcessStartInfo psi = new()
        {
            FileName = "explorer.exe",
            Arguments = arguments,
            UseShellExecute = true // Required in .NET Core and later
        };

        Process.Start(psi);
    }
}
