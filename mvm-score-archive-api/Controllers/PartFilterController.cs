using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.PartFilter;

namespace Mvm.Score.Archive.Api.Controllers;

public class PartFilterController : ApiControllerBase
{
    private readonly IPartFilterService partFilterService;

    public PartFilterController(
        IPartFilterService partFilterService)
    {
        this.partFilterService = partFilterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPartFiltersAsync(CancellationToken cancellationToken)
    {
        Guid? userIdParsed = this.GetUserIdAsGuid();

        var result = await this.partFilterService.GetAllFiltersForUserAsync(userIdParsed, cancellationToken);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ReturnProblemDetail(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> AddPartFilterAsync(PartFilterIncomingDto partFilterDto, CancellationToken cancellationToken)
    {
        Guid? userIdParsed = this.GetUserIdAsGuid();

        string? userName = this.User.FindFirst("name")?.Value;

        int newPartFilterId = await this.partFilterService.CreateNewFilterAsync(userIdParsed, userName, partFilterDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{newPartFilterId}", newPartFilterId);
    }

    private Guid? GetUserIdAsGuid()
    {
        string? userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId != null
            ? Guid.Parse(userId)
            : null;
    }
}