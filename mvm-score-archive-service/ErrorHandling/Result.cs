namespace Mvm.Score.Archive.Service.ErrorHandling;

public class Result<T>
{
    private readonly T value;

    private Result(T value)
    {
        this.Value = value;
        this.IsSuccess = true;
        this.Error = CustomError.None;
    }

    private Result(CustomError error)
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
