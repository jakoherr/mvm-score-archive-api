using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class ScoreErrors
{
    public static readonly CustomError NoScoresFound = new CustomError(
        "Score.NotFound", "No scores found in database.", HttpStatusCode.NotFound);

    public static CustomError ScoreNotFound(int scoreId) => new CustomError(
        "Score.NotFound", $"The score with id {scoreId} cannot be found in the database.", HttpStatusCode.NotFound);
}
