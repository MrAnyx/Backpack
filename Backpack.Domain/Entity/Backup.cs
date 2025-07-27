using Backpack.Domain.Contract.Persistence;
using System;
using System.Collections.Generic;

namespace Backpack.Domain.Entity;

public class Backup : Model.Entity, IHasTimestamps
{
    public required string Name { get; set; }
    public required bool Overwrite { get; set; }
    public required string Ignore { get; set; }
    public required string SourcePath { get; set; }
    public required string DestinationPath { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

#nullable disable
    public virtual ICollection<Profile> Profiles { get; set; } = [];
#nullable enable
}
