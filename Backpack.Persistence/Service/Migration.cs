﻿using Backpack.Domain.Contract.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Persistence.Service;

public class Migration(ApplicationDbContext _context) : IMigration
{
    public Task<IEnumerable<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.GetPendingMigrationsAsync(cancellationToken);
    }

    public Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.MigrateAsync(cancellationToken);
    }
}
