using System.Runtime.InteropServices;
using System.Security;

namespace Backpack.Shared.Extension;

public static class SecureStringExtension
{
    public static string ToUnsecureString(this SecureString secureString)
    {
        var unmanagedString = nint.Zero;

        try
        {
            // Convert SecureString to unmanaged memory
            unmanagedString = Marshal.SecureStringToBSTR(secureString);
            // Convert unmanaged memory to a normal string
            return Marshal.PtrToStringBSTR(unmanagedString);
        }
        finally
        {
            if (unmanagedString != nint.Zero)
            {
                Marshal.ZeroFreeBSTR(unmanagedString); // Clean up unmanaged memory
            }
        }
    }
}
