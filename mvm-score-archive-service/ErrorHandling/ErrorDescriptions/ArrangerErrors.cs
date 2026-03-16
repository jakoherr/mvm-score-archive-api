using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class ArrangerErrors
{
    public static readonly CustomError NoArrangersFound = new CustomError(
        "Arrangers.NotFound", "No arrangers found in database.", HttpStatusCode.NotFound);

    public static CustomError ArrangerNotFound(int id) => new CustomError(
        "Arranger.NotFound", $"The arranger with id {id} was not found.", HttpStatusCode.NotFound);
}
