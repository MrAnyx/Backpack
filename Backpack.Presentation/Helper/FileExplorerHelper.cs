using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;

namespace Backpack.Presentation.Helper;

public static class FileExplorerHelper
{
    public static bool TryExploreFile([NotNullWhen(true)] out string? path)
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
}
