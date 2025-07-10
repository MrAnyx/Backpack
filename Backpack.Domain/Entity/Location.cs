using Backpack.Domain.Contract.Persistence;

namespace Backpack.Domain.Entity;

public abstract class Location : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public required uint SourceBackupId { get; set; }
    public required uint DestinationBackupId { get; set; }

#nullable disable
    public virtual Backup SourceBackup { get; set; }
    public virtual Backup DestinationBackup { get; set; }
#nullable enable
}

