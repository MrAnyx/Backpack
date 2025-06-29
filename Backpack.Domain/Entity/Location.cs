using Backpack.Domain.Persistence.Contract;

namespace Backpack.Domain.Entity;

public class Location : Persistence.Entity, IHasTimestamps
{
    public required string Path { get; set; }
    public IEnumerable<string> IgnoreWildcards { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
