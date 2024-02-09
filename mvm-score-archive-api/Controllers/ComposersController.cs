using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Composers;

namespace Mvm.Score.Archive.Api.Controllers;

public class ComposersController : ApiControllerBase
{
    public const string ComposerPath = "/composer";

    private readonly IComposerService composerService;

    public ComposersController(IComposerService composerService)
    {
        this.composerService = composerService;
    }

    [HttpPost(ComposerPath)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComposerAsync([FromBody] ComposerDto composerDto, CancellationToken cancellationToken)
    {
        int id = await this.composerService.AddComposer(cancellationToken, composerDto);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }
}
