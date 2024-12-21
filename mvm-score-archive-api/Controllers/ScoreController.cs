using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Score;

namespace Mvm.Score.Archive.Api.Controllers;

public class ScoreController : ApiControllerBase
{
    private readonly IScoreService scoreService;

    public ScoreController(
        IScoreService scoreService)
    {
        this.scoreService = scoreService;
    }

    [HttpPost]
    public async Task<IActionResult> AddScoreAsync([FromBody] IncomingScoreDto scoreDto, CancellationToken cancellationToken)
    {
        int id = await this.scoreService.AddScoreAsync(scoreDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }
}
