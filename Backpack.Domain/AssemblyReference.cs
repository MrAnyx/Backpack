using System.Reflection;

namespace Backpack.Domain;

public class AssemblyReference
{
    public static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
