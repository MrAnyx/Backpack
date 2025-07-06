using Backpack.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backpack.Domain.Configuration;

public class AppSettings
{
    [Required]
    public required eAppEnvironment Environment { get; init; }
}
