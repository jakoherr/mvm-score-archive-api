namespace Mvm.Score.Archive.Service.ErrorHandling;

public class Result<T>
{
    private readonly T value;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Result(T value)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        this.Value = value;
        this.IsSuccess = true;
        this.Error = CustomError.None;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Result(CustomError error)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        if (error == CustomError.None)
        {
            throw new ArgumentException("invalid error", nameof(error));
        }

        this.IsSuccess = false;
        this.Error = error;
    }

    public T Value
    {
        get
        {
            if (this.IsFailure)
            {
                throw new InvalidOperationException("there is no value for failure");
            }

            return this.value!;
        }

        private init => this.value = value;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !this.IsSuccess;

    public CustomError Error { get; }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(CustomError error) => new(error);
}
