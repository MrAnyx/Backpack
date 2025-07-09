using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

public class Location : Model.Entity, IHasTimestamps
{
    public required string Path { get; set; }
    public IEnumerable<string> IgnoreWildcards { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

#nullable disable
    public virtual ICollection<Backup> SourceBackups { get; set; }
    public virtual ICollection<Backup> DestinationBackups { get; set; }
#nullable enable
}
