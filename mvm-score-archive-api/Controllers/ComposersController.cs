using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Composer;

namespace Mvm.Score.Archive.Api.Controllers;

public class ComposersController : ApiControllerBase
{
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
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComposerAsync([FromBody] IncomingComposerDto composerDto, CancellationToken cancellationToken)
    {
        int id = await this.composerService.AddComposerAsync(composerDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }

    /// <summary>
    /// Gets all composers in database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all composers.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OutgoingComposerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComposersAsync(CancellationToken cancellationToken)
    {
        var composers = await this.composerService.GetComposersAsync(cancellationToken);

        return !composers.Any() ? this.NotFound() : this.Ok(composers);
    }

    /// <summary>
    /// Gets a composer provided by id.
    /// </summary>
    /// <param name="id">The id of the composer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The composer</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OutgoingComposerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComposerAsync(int id, CancellationToken cancellationToken)
    {
        var composers = await this.composerService.GetComposerByIdAsync(id, cancellationToken);

        return composers is null ? this.NotFound() : this.Ok(composers);
    }

    /// <summary>
    /// Deletes a composer by its id.
    /// </summary>
    /// <param name="id">The id of the composer</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComposerById(int id, CancellationToken cancellationToken)
    {
        await this.composerService.DeleteComposerByIdAsync(id, cancellationToken);

        return this.NoContent();
    }

}
