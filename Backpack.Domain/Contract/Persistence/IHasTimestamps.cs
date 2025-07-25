﻿using System;

namespace Backpack.Domain.Contract.Persistence;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
