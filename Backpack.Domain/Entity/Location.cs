using Backpack.Domain.Contract;

namespace Backpack.Domain.Entity;

public class Location : Model.Entity, IHasTimestamps
{
    public required string Path { get; set; }
    public IEnumerable<string> IgnoreWildcards { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
