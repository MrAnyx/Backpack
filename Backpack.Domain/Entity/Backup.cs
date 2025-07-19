using Backpack.Domain.Attribute;
using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

/// <summary>
/// An atomic backup couple with a source location and a destination
/// </summary>
public class Backup : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }
    public required bool Overwrite { get; set; }
    public required string Ignore { get; set; }
    public required string SourcePath { get; set; }
    public required string DestinationPath { get; set; }

    [IgnoreMerge]
    public DateTime CreatedAt { get; set; }

    [IgnoreMerge]
    public DateTime UpdatedAt { get; set; }

#nullable disable
    [IgnoreMerge]
    public virtual ICollection<Profile> Profiles { get; set; } = [];
#nullable enable
}
