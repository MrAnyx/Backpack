using System;

namespace Backpack.Domain.Model;
public class PipelineContext
{
    public Guid RequestId { get; } = Guid.NewGuid();
    public Guid HandlerId { get; init; } = Guid.NewGuid();
    public DateTime UtcTimestamp { get; } = DateTime.UtcNow;
}
