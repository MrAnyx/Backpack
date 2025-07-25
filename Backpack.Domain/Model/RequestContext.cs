using System;

namespace Backpack.Domain.Model;
public class RequestContext
{
    public Guid CorrelationId { get; } = Guid.NewGuid();
    public DateTime UtcTimestamp { get; } = DateTime.UtcNow;
}
