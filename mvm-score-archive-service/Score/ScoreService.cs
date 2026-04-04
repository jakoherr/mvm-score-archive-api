using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ErrorHandling;
using Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;
using Mvm.Score.Archive.Service.Files;
using Mvm.Score.Archive.Service.Parts;
using Mvm.Score.Archive.Service.PdfGenerator;

namespace Mvm.Score.Archive.Service.Score;

public class ScoreService : IScoreService
{
    private readonly ILogger<ScoreService> logger;
    private readonly IMapper mapper;
    private readonly AppDbContext dbContext;
    private readonly IFileService fileService;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IPdfGeneratorService pdfGeneratorService;

    public ScoreService(
        ILogger<ScoreService> logger,
        IMapper mapper,
        AppDbContext dbContext,
        IFileService fileService,
        IHttpClientFactory httpClientFactory,
        IPdfGeneratorService pdfGeneratorService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.dbContext = dbContext;
        this.fileService = fileService;
        this.httpClientFactory = httpClientFactory;
        this.pdfGeneratorService = pdfGeneratorService;
    }

    public async Task<int> AddScoreAsync(IncomingScoreDto incomingScoreDto, CancellationToken cancellationToken)
    {
        DbScore dbScore = this.mapper.Map<DbScore>(incomingScoreDto);

        this.dbContext.Scores.Add(dbScore);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        dbScore.FilePath = this.fileService.CreateScoreFolder($"{dbScore.Title} {dbScore.Id}");
        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogInformation("New score added: {@Score}", dbScore);

        return dbScore.Id;
    }

    public async Task<Result<int>> DeleteScoreAndFilesByIdAsync(int scoreId, CancellationToken cancellationToken)
    {
        DbScore? dbScore = this.dbContext.Scores
            .FirstOrDefault(s => s.Id == scoreId);

        if (dbScore is null)
        {
            return Result<int>.Failure(ScoreErrors.ScoreNotFound(scoreId));
        }

        int deletedScores = await this.dbContext.Scores
            .Where(s => s.Id == scoreId)
            .ExecuteDeleteAsync(cancellationToken);

        this.fileService.DeleteFolderAndFiles(dbScore.FilePath);

        this.logger.LogInformation("The score with Name {ScoreName} was deleted.", dbScore.Title);

        return Result<int>.Success(deletedScores);
    }

    public async Task<Result<int>> AddScoreFileAsync(
        IFormFile file,
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        DbScore? dbScore = this.dbContext.Scores
            .Include(p => p.Parts)
            .FirstOrDefault(s => s.Id == scoreId);

        if (dbScore is null)
        {
            return Result<int>.Failure(ScoreErrors.ScoreNotFound(scoreId));
        }

        DbPart? dbPart = this.dbContext.Parts.FirstOrDefault(s => s.Id == partId);

        if (dbPart is null)
        {
            return Result<int>.Failure(PartErrors.PartNotFound(partId));
        }

        dbScore.Parts.Add(dbPart);

        await this.fileService.RenameAndStoreFileAsync(file, dbScore.FilePath, dbPart.FileName, cancellationToken);

        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogDebug("New file was saved.");

        return Result<int>
            .Success(scoreId);
    }

    public async Task<Result<StreamFile>> ReadSingleScoreFileAsync(
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        DbScore? dbScore = this.dbContext.Scores
            .Include(p => p.Parts)
            .FirstOrDefault(s => s.Id == scoreId);

        if (dbScore is null)
        {
            return Result<StreamFile>.Failure(ScoreErrors.ScoreNotFound(scoreId));
        }

        DbPart? dbPartInScore = dbScore.Parts.FirstOrDefault(p => p.Id == partId);
        Part originPart = new();

        if (dbPartInScore is null)
        {
            DbPart? dbPart = this.dbContext.Parts.FirstOrDefault(p => p.Id == partId);
            if (dbPart is null)
            {
                return Result<StreamFile>.Failure(PartErrors.PartNotFound(partId));
            }

            originPart = this.mapper.Map<Part>(dbPart);

            while (dbPart.FallbackPartId is not null)
            {
                if (dbScore.Parts.Select(x => x.Id).Contains(dbPart.FallbackPartId.Value))
                {
                    dbPartInScore = dbScore.Parts.First(x => x.Id == dbPart.FallbackPartId.Value);
                    break;
                }

                dbPart = this.dbContext.Parts.FirstOrDefault(p => p.Id == dbPart.FallbackPartId);

                if (dbPart is null)
                {
                    return Result<StreamFile>.Failure(PartErrors.PartNotFound(partId));
                }
            }
        }

        if (dbPartInScore is null)
        {
            var file = this.pdfGeneratorService.GeneratePartPdfAsStream(originPart);
            return Result<StreamFile>.Success(new StreamFile(file, originPart));
        }

        var stream = await this.fileService.ReadFileFromDiskAsync(Path.Combine(dbScore.FilePath, dbPartInScore.FileName), cancellationToken);

        return Result<StreamFile>
            .Success(new StreamFile(stream, this.mapper.Map<Part>(dbPartInScore)));
    }

    public async Task<Result<Stream>> ReadAllFilesAndMergeAsync(
        int scoreId,
        IncomingPartMerge partMerge,
        CancellationToken cancellationToken)
    {
        List<StreamFile> fileStreams = new();

        foreach (var part in partMerge.PartAmounts)
        {
            for (int i = 0; i < part.Value; i++)
            {
                var streamFileResult = await this.ReadSingleScoreFileAsync(scoreId, part.Key, cancellationToken);
                if (streamFileResult.IsSuccess)
                {
                    fileStreams.Add(streamFileResult.Value);
                    continue;
                }

                return Result<Stream>.Failure(streamFileResult.Error);
            }
        }

        var response = await this.InvokeStirlingPdf(cancellationToken, fileStreams);

        if (!response.IsSuccessStatusCode)
        {
            return Result<Stream>.Failure(PartErrors.StirlingPdfNotReachable);
        }

        Stream responseBody = await response.Content.ReadAsStreamAsync(cancellationToken);
        return Result<Stream>.Success(responseBody);
    }

    public async Task<Result<IReadOnlyCollection<OutgoingScoreDto>>> GetScoresAsync(CancellationToken cancellationToken)
    {
        var dbScores = await this.dbContext.Scores
            .Include(p => p.Composer)
            .Include(p => p.Arranger)
            .Include(p => p.Parts)
            .ToListAsync(cancellationToken);

        if (!dbScores.Any())
        {
            return Result<IReadOnlyCollection<OutgoingScoreDto>>.Failure(ScoreErrors.NoScoresFound);
        }

        return Result<IReadOnlyCollection<OutgoingScoreDto>>
            .Success(this.mapper.Map<IReadOnlyCollection<OutgoingScoreDto>>(dbScores));
    }

    public async Task<Result<OutgoingScoreDto>> GetScoreByIdAsync(int scoreId, CancellationToken cancellationToken)
    {
        DbScore? dbScore = await this.dbContext.Scores
            .Include(p => p.Composer)
            .Include(p => p.Arranger)
            .Include(p => p.Parts.OrderBy(part => part.SortOrder))
            .FirstOrDefaultAsync(s => s.Id == scoreId, cancellationToken);

        if (dbScore == null)
        {
            return Result<OutgoingScoreDto>.Failure(ScoreErrors.ScoreNotFound(scoreId));
        }

        return Result<OutgoingScoreDto>
            .Success(this.mapper.Map<OutgoingScoreDto>(dbScore));
    }

    public async Task<Result<bool>> DeleteScorePartAsync(int scoreId, int partId, CancellationToken cancellationToken)
    {
        var score = await this.dbContext.Scores
            .Include(s => s.Parts)
            .FirstOrDefaultAsync(s => s.Id == scoreId, cancellationToken);

        var dbPart = score?.Parts.FirstOrDefault(p => p.Id == partId);

        if (dbPart == null)
        {
            return Result<bool>.Failure(PartErrors.PartNotFound(partId));
        }

        try
        {
            this.fileService.DeleteFileByPath(Path.Combine(score!.FilePath, dbPart.FileName));
        }
        catch
        {
            return Result<bool>.Failure(FileErrors.FileDeleteError);
        }

        score!.Parts.Remove(dbPart);
        await this.dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }

    private async Task<HttpResponseMessage> InvokeStirlingPdf(CancellationToken cancellationToken, List<StreamFile> fileStreams)
    {
        using var httpClient = this.httpClientFactory.CreateClient("StrilingPdf");
        using var formData = new MultipartFormDataContent();

        foreach (var fileStream in fileStreams.OrderBy(x => x.PartInformation.SortOrder))
        {
            var fileContent = new StreamContent(fileStream.Stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            formData.Add(fileContent, "fileInput", fileStream.PartInformation.FileName);
        }

        var response = await httpClient.PostAsync("general/merge-pdfs", formData, cancellationToken);
        return response;
    }
}