using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

public class Profile : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

#nullable disable
    public virtual ICollection<Backup> Backups { get; set; } = [];
    public virtual ICollection<History> Histories { get; set; } = [];
#nullable enable
}
