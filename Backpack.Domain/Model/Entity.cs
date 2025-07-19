using Backpack.Domain.Attribute;

namespace Backpack.Domain.Model;

public abstract class Entity
{
    [IgnoreMerge]
    public uint Id { get; set; }
}
