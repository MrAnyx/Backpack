using System.Reflection;

namespace Backpack.Infrastructure;

public class AssemblyReference
{
    public static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
