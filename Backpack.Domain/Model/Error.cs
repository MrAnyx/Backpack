namespace Backpack.Domain.Model;

public sealed record Error(Exception? error)
{
    public static readonly Error None = new(error: null);
}