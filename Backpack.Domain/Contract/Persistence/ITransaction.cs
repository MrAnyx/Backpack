﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract.Persistence;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}