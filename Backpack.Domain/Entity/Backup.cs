using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

/// <summary>
/// An atomic backup couple with a source location and a destination
/// </summary>
public class Backup : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }
    public required bool Overwrite { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public required uint SourceId { get; set; }
    public required uint DestinationId { get; set; }

#nullable disable
    public virtual Location Source { get; set; }
    public virtual Location Destination { get; set; }
    public virtual ICollection<Wildcard> Wildcards { get; set; } = [];
    public virtual ICollection<Profile> Profiles { get; set; } = [];
#nullable enable
}
