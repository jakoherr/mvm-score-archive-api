using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Composers;

namespace Mvm.Score.Archive.Api.Controllers;

public class ComposersController : ApiControllerBase
{
    public const string ComposerPath = "api/composer";

    public const string ComposersPath = "api/composers";

    private readonly IComposerService composerService;

    public ComposersController(IComposerService composerService)
    {
        this.composerService = composerService;
    }

    /// <summary>
    /// Adds a new composer to the database.
    /// </summary>
    /// <param name="composerDto">The composer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id for the new composer.</returns>
    [HttpPost(ComposerPath)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComposerAsync([FromBody] IncomingComposerDto composerDto, CancellationToken cancellationToken)
    {
        int id = await this.composerService.AddComposerAsync(cancellationToken, composerDto);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }

    /// <summary>
    /// Gets all composers in database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all composers.</returns>
    [HttpGet(ComposersPath)]
    [ProducesResponseType(typeof(IReadOnlyList<OutgoingComposerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComposersAsync(CancellationToken cancellationToken)
    {
        var composers = await this.composerService.GetComposersAsync(cancellationToken);

        return !composers.Any() ? this.NotFound() : this.Ok(composers);
    }
}
