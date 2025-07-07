using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Enum;

namespace Backpack.Domain.Entity;

public class Backup : Model.Entity, IHasTimestamps
{
    public required eBackupTrigger Trigger { get; set; }
    public required eBackupType Type { get; set; }
    public required eBackupStatus Status { get; set; }
    public required ulong FileCount { get; set; }
    public required ulong Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

#nullable disable
    public virtual Location Source { get; set; }
    public virtual Location Destination { get; set; }
#nullable enable
}
