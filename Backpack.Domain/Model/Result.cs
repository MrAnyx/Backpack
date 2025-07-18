namespace Backpack.Domain.Model;

public class Result { }

public class Result<T>(T value) : Result
{
    public T Value { get; } = value;

    public static implicit operator Result<T>(T value) => new(value);
}