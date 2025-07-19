namespace Backpack.Domain.Attribute;
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]

public sealed class IgnoreMergeAttribute() : System.Attribute
{
}
