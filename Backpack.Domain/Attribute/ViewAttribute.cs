using System;

namespace Backpack.Domain.Attribute;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ViewAttribute(Type viewType) : System.Attribute
{
    public Type ViewType { get; } = viewType;
}
