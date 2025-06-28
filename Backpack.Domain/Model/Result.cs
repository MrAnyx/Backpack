namespace Backpack.Domain.Model;

public class Result
{
    public bool IsSuccess => Error == Error.None;
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private Result(Error error)
    {
        Error = error;
    }

    public static Result Success() => new(Error.None);
    public static Result Failure(Error error) => new(error);
}
