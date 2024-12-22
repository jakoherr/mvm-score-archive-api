using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Arranger;

namespace Mvm.Score.Archive.Api.Controllers;

public class ArrangerController : ApiControllerBase
{
    private readonly IArrangerService arrangerService;

    public ArrangerController(
        IArrangerService arrangerService)
    {
        this.arrangerService = arrangerService;
    }

    /// <summary>
    /// Adds a new arranger to the database.
    /// </summary>
    /// <param name="arrangerDto">The arranger.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id for the new arranger.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddArrangerAsync([FromBody] IncomingArrangerDto arrangerDto, CancellationToken cancellationToken)
    {
        int id = await this.arrangerService.AddArrangerAsync(arrangerDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }

    /// <summary>
    /// Gets all arrangers in database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>List.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OutgoingArrangerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArrangersAsync(CancellationToken cancellationToken)
    {
        var arrangers = await this.arrangerService.GetArrangerAsync(cancellationToken);

        return !arrangers.Any()
            ? this.NotFound()
            : this.Ok(arrangers);
    }

    /// <summary>
    /// Gets a arranger provided by id.
    /// </summary>
    /// <param name="id">The id of the arranger.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The arranger.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OutgoingArrangerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArrangerByIdAsync(int id, CancellationToken cancellationToken)
    {
        var arranger = await this.arrangerService.GetArrangerByIdAsync(id, cancellationToken);

        return arranger is null
            ? this.NotFound()
            : this.Ok(arranger);
    }

    /// <summary>
    /// Deletes an arranger by its id.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArrangerByIdAsync(int id, CancellationToken cancellationToken)
    {
        await this.arrangerService.DeleteArrangerByIdAsync(id, cancellationToken);

        return this.NoContent();
    }
}
