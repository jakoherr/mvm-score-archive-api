using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class FileErrors
{
    public static readonly CustomError FileDeleteError = new CustomError(
            "File.DeleteError", "The file could not be deleted.", HttpStatusCode.BadRequest);
}