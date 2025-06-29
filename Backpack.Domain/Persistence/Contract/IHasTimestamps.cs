﻿namespace Backpack.Domain.Persistence.Contract;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
