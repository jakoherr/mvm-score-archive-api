using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ErrorHandling;
using Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;
using Mvm.Score.Archive.Service.Files;

namespace Mvm.Score.Archive.Service.Score;

public class ScoreService : IScoreService
{
    private readonly ILogger<ScoreService> logger;
    private readonly IMapper mapper;
    private readonly AppDbContext dbContext;
    private readonly IFileService fileService;

    public ScoreService(
        ILogger<ScoreService> logger,
        IMapper mapper,
        AppDbContext dbContext,
        IFileService fileService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.dbContext = dbContext;
        this.fileService = fileService;
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

        DbPart? dbPart = dbScore.Parts.FirstOrDefault(p => p.Id == partId);

        if (dbPart is null)
        {
            return Result<StreamFile>.Failure(PartErrors.PartInScoreNotFound(partId, scoreId));
        }

        var stream = await this.fileService.ReadFileFromDiskAsync(Path.Combine(dbScore.FilePath, dbPart.FileName), cancellationToken);

        return Result<StreamFile>
            .Success(new StreamFile(dbPart.FileName, stream));
    }

    public async Task<Result<IReadOnlyCollection<OutgoingScoreDto>>> GetScoresAsync(CancellationToken cancellationToken)
    {
        var dbScores = await this.dbContext.Scores
            .Include(p => p.Composer)
            .Include(p => p.Arranger)
            .ToListAsync(cancellationToken);

        if (!dbScores.Any())
        {
            return Result<IReadOnlyCollection<OutgoingScoreDto>>.Failure(ScoreErrors.NoScoresFound);
        }

        return Result<IReadOnlyCollection<OutgoingScoreDto>>
            .Success(this.mapper.Map<IReadOnlyCollection<OutgoingScoreDto>>(dbScores));
    }
}
