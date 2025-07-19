using Backpack.Domain.Attribute;
using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

public class Profile : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    [IgnoreMerge]
    public DateTime CreatedAt { get; set; }

    [IgnoreMerge]
    public DateTime UpdatedAt { get; set; }

#nullable disable
    [IgnoreMerge]
    public virtual ICollection<Backup> Backups { get; set; } = [];

    [IgnoreMerge]
    public virtual ICollection<History> Histories { get; set; } = [];
#nullable enable
}
