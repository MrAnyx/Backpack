using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Enum;
using System;

namespace Backpack.Domain.Entity;
public class History : Model.Entity, IHasTimestamps
{
    public required TimeSpan Duration { get; set; }
    public required long FileCount { get; set; }
    public required long TotalBytes { get; set; }

    public required eBackupStatus Status { get; set; }
    public required eBackupTrigger Trigger { get; set; }
    public required eBackupType Type { get; set; }
    public required uint ProfileId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


#nullable disable
    public virtual Profile Profile { get; set; }
#nullable enable
}
