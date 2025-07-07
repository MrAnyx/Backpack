namespace Backpack.Domain.Model;

public class Result { }

public class Result<T> : Result
{
    public T Value { get; }

    private Result(T value)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T value) => new(value);
}