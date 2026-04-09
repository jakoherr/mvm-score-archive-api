using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class PartFilterErrors
{
    public static readonly CustomError NoPartFilters = new CustomError(
        "PartFilter.NotFound", "No part filters found in database.", HttpStatusCode.NotFound);

    public static CustomError PartFilterNotFound(int partFilterId) => new CustomError(
        "PartFilter.NotFound", $"The part filter with id {partFilterId} cannot be found in the database.", HttpStatusCode.NotFound);
}