namespace Mvm.Score.Archive.Service.Exceptions;

public class NotFoundException : Exception
{
    public string Error { get; }

    public string Message { get; }

    public NotFoundException(string error, string message) : base(message)
    {
        this.Error = error;
        this.Message = message;
    }
}
