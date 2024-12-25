using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Parts;

namespace Mvm.Score.Archive.Api.Controllers;

public class PartController : ApiControllerBase
{
    private readonly IPartsService partsService;

    public PartController(IPartsService partsService)
    {
        this.partsService = partsService;
    }

    /// <summary>
    /// Gets all parts in database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetPartsAsync(CancellationToken cancellationToken)
    {
        var parts = await this.partsService.GetPartsAsync(cancellationToken);

        return this.Ok(parts);
    }

    /// <summary>
    /// Adds a new part to the database.
    /// </summary>
    /// <param name="incommingPartDto">Part.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddPartAsync(IncommingPartDto incommingPartDto, CancellationToken cancellationToken)
    {
        int partId = await this.partsService.AddPartAsync(incommingPartDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{partId}", partId);
    }
}
