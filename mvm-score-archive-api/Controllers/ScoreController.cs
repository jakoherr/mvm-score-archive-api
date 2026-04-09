using System.Net.Mime;
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
    /// Returns a single score by the provided id.
    /// </summary>
    /// <param name="id">The id of the score.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The score.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScoreByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await this.scoreService.GetScoreByIdAsync(id, cancellationToken);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Deletes the score from the database and removes all files from the disk.
    /// </summary>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="cancellationToken">The cancellation Token.</param>
    /// <returns>The numbers of deletes scores.</returns>
    [HttpDelete("{scoreId}")]
    public async Task<IActionResult> DeleteScoreAndFilesByIdAsync(int scoreId, CancellationToken cancellationToken)
    {
        var result = await this.scoreService.DeleteScoreAndFilesByIdAsync(scoreId, cancellationToken);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Endpoint to upload the pdf file.
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
    /// Deletes a Part of a score in database and the pdf file.
    /// </summary>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="partId">The id of the part.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>NoContent.</returns>
    [HttpDelete("{scoreId}/{partId}")]
    public async Task<IActionResult> DeletePartAndFileAsync(
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        var result = await this.scoreService.DeleteScorePartAsync(scoreId, partId, cancellationToken);
        return result.IsSuccess
            ? this.NoContent()
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
        this.Response.Headers.Append("Content-Disposition", $"inline; filename={result.Value.PartInformation.FileName}");

        return result.IsSuccess
            ? this.File(result.Value.Stream, MediaTypeNames.Application.Pdf)
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Mergs the parts of a score.
    /// </summary>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="mergeDicitonary">The dicionary for merging.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The merged pdf.</returns>
    [HttpPost("file/{scoreId}")]
    public async Task<IActionResult> GetMergedPdfsAsync(int scoreId, IncomingPartMerge mergeDicitonary, CancellationToken cancellationToken)
    {
        var result = await this.scoreService.ReadAllFilesAndMergeAsync(scoreId, mergeDicitonary, cancellationToken);

        return result.IsSuccess
            ? this.File(result.Value, "application/pdf")
            : this.ReturnProblemDetail(result.Error);
    }

    /// <summary>
    /// Merges the pdfs based on a filter.
    /// </summary>
    /// <param name="scoreId">The id of the score.</param>
    /// <param name="filterId">The id of the part filter.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The file.</returns>
    [HttpGet("file/filtered/{scoreId:int}/{filterId:int}")]
    public async Task<IActionResult> GetMergedPdfByPartFilterIdAsync(int scoreId, int filterId, CancellationToken cancellationToken)
    {
        var result = await this.scoreService.MergeFilesByFilterIdAsync(scoreId, filterId, cancellationToken);

        return result.IsSuccess
            ? this.File(result.Value, "application/pdf")
            : this.ReturnProblemDetail(result.Error);
    }
}
