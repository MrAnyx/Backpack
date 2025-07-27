using Backpack.Domain.Attribute;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Backpack.Presentation.Helper;
public static class BindingHelper
{
    public static void BindTriggerCanExecute(object dto, object viewModel)
    {
        var dtoType = dto.GetType();
        var vmType = viewModel.GetType();

        if (dto is not INotifyPropertyChanged notifier)
        {
            return;
        }

        var propertyToCommands = dto.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .SelectMany(field => field.GetCustomAttributes(typeof(NotifyCanExecuteChangedForAttribute), true)
                .Cast<NotifyCanExecuteChangedForAttribute>()
                .Select(attr => new { FieldName = field.Name, attr.CommandName }))
            .GroupBy(x => x.CommandName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => ToPascalCase(x.FieldName.TrimStart('_'))).ToHashSet()
            );

        notifier.PropertyChanged += (_, e) =>
        {
            foreach (var kvp in propertyToCommands)
            {
                if (kvp.Value.Contains(e.PropertyName!))
                {
                    var command = viewModel.GetType()
                        .GetProperty(kvp.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        ?.GetValue(viewModel) as IRelayCommand;

                    command?.NotifyCanExecuteChanged();
                }
            }
        };
    }

    // Convert field name like '_hostname' or 'hostname' to 'Hostname'
    private static string ToPascalCase(string name)
    {
        return string.Join("", name
            .Split(['_', '-', ' '], StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1)));
    }
}
