using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

public class Wildcard : Model.Entity, IHasTimestamps
{
    public required string Pattern { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public required uint BackupId { get; set; }

#nullable disable
    public virtual Backup Backup { get; set; }
#nullable enable
}
