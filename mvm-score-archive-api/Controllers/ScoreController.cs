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

    /// <summary>
    /// Adds a score to the database and creates the folder.
    /// </summary>
    /// <param name="scoreDto">The score dto.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The id of the score.</returns>
    [HttpPost]
    public async Task<IActionResult> AddScoreAsync([FromBody] IncomingScoreDto scoreDto, CancellationToken cancellationToken)
    {
        int id = await this.scoreService.AddScoreAsync(scoreDto, cancellationToken);

        return this.Created($"{this.HttpContext.Request.GetEncodedUrl()}/{id}", id);
    }

    /// <summary>
    /// Returns all Scores in the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Scores.</returns>
    [HttpGet]
    public async Task<IActionResult> GetScoresAsync(CancellationToken cancellationToken)
    {
        var result = await this.scoreService.GetScoresAsync(cancellationToken);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Endpoint to upload the pdf file
    /// </summary>
    /// <param name="file">The PDF file.</param>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="partId">The id of the part.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created.</returns>
    [HttpPost("upload/{scoreId}/{partId}")]
    public async Task<IActionResult> UploadFileAsync(
        IFormFile file,
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        var result = await this.scoreService.AddScoreFileAsync(file, scoreId, partId, cancellationToken);

        return result.IsSuccess
            ? this.Created()
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Gets a PDF file for a provided score and part.
    /// </summary>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="partId">The id of the part.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>The PDF file of the part.</returns>
    [HttpGet("file/{scoreId}/{partId}")]
    public async Task<IActionResult> GetFileByIdAsync(
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        var result = await this.scoreService.ReadSingleScoreFileAsync(scoreId, partId, cancellationToken);

        return result.IsSuccess
            ? this.File(result.Value.Stream, "application/pdf", result.Value.FileName)
            : this.ReturnProblemDetail(result.Error);
    }
}
