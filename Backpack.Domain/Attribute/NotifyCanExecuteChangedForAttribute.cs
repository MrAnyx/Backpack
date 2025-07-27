using System;

namespace Backpack.Domain.Attribute;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class NotifyCanExecuteChangedForAttribute : System.Attribute
{
    public string CommandName { get; }

    public NotifyCanExecuteChangedForAttribute(string commandName)
    {
        CommandName = commandName;
    }
}