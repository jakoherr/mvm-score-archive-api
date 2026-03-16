using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling;

public sealed record CustomError(string Title, string Description, HttpStatusCode HttpStatusCode)
{
    public static readonly CustomError None = new(string.Empty, string.Empty, HttpStatusCode.InternalServerError);
}