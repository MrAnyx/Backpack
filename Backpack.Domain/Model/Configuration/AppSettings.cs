using Backpack.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backpack.Domain.Model.Configuration;
public class AppSettings
{
    [Required]
    public required eAppConfiguration Environment { get; init; }
}
