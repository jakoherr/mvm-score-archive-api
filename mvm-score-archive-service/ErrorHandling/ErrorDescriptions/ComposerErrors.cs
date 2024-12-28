using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class ComposerErrors
{
    public static CustomError ComposerNotFound(int id) => new CustomError(
        "Composer.NotFound", $"The composer with id {id} was not found.", HttpStatusCode.NotFound);

    public static readonly CustomError NoComposersFound = new CustomError(
        "Composers.NotFound", "No composers found in database.", HttpStatusCode.NotFound);
}
