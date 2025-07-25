using Backpack.Domain.Attribute;
using System;
using System.Reflection;

namespace Backpack.Shared.Extension;

public static class MergeExtension
{
    public static void MergeWith<T>(this T target, T source)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (source == null)
        {
            return;
        }

        var type = typeof(T);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead || !prop.CanWrite)
            {
                continue;
            }

            if (prop.GetCustomAttribute<IgnoreMergeAttribute>() is not null)
            {
                continue;
            }

            var value = prop.GetValue(source);
            prop.SetValue(target, value);
        }
    }
}
