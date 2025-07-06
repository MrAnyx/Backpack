using System.Diagnostics.CodeAnalysis;

namespace Backpack.Domain.Model;

public class Result
{
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess => Exception == null;

    [MemberNotNullWhen(true, nameof(Exception))]
    public bool IsFailure => !IsSuccess;

    public Exception? Exception { get; }

    protected Result(Exception? exception)
    {
        Exception = exception;
    }

    public static Result Success() => new(null);
    public static Result Failure(Exception exception) => new(exception);

    public static implicit operator Result(Exception exception) => Failure(exception);
}

public class Result<T> : Result
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Exception))]
    public new bool IsSuccess => base.IsSuccess && Value is not null;

    [MemberNotNullWhen(true, nameof(Exception))]
    [MemberNotNullWhen(false, nameof(Value))]
    public new bool IsFailure => !IsSuccess;

    public new Exception? Exception { get; }

    public T? Value { get; }

    private Result(T? value, Exception? exception) : base(exception)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, null);
    public static new Result<T> Failure(Exception exception) => new(default, exception);

    // Implicit from T to Result<T>
    public static implicit operator Result<T>(T value) => Success(value);

    // Implicit from Exception to Result<T>
    public static implicit operator Result<T>(Exception exception) => Failure(exception);
}