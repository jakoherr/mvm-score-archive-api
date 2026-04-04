using System.Net;

namespace Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

public class PartFilterErrors
{
    public static readonly CustomError NoPartFilters = new CustomError(
        "PartFilter.NotFound", "No part filters found in database.", HttpStatusCode.NotFound);
}