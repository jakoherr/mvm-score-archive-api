using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class PartErrors
{
    public static readonly CustomError NoPartFound = new CustomError(
    "Part.NotFound", "No parts found in database.", HttpStatusCode.NotFound);

    public static readonly CustomError StirlingPdfNotReachable = new CustomError(
        "StirlingPDF", "Stirling PDF failed.", HttpStatusCode.BadRequest);

    public static CustomError PartNotFound(int partId) => new CustomError(
    "Part.NotFound", $"The part with id {partId} cannot be found in the database.", HttpStatusCode.NotFound);

    public static CustomError PartInScoreNotFound(int partId, int scoreId) => new CustomError(
    "PartInScore.NotFound", $"The part with id {partId} cannot be found in score {scoreId}.", HttpStatusCode.NotFound);
}
