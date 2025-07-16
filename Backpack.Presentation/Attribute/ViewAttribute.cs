namespace Backpack.Presentation.Attribute;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ViewAttribute(Type viewType) : System.Attribute
{
    public Type ViewType { get; } = viewType;
}
