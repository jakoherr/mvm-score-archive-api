using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Exceptions;
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

        dbScore.FilePath = this.fileService.CreateScoreFolder(dbScore.Title);

        this.logger.LogInformation("New score added: {@Score}", dbScore);

        return dbScore.Id;
    }

    public async Task AddScoreFileAsync(
        IFormFile file,
        int scoreId,
        int partId,
        CancellationToken cancellationToken)
    {
        DbScore dbScore = this.dbContext.Scores
            .Include(p => p.Parts)
            .FirstOrDefault(s => s.Id == scoreId)
            ?? throw new Exception();

        DbPart dbPart = dbScore.Parts.FirstOrDefault(s => s.Id == partId) ?? throw new Exception();

        await this.fileService.RenameAndStoreFileAsync(file, dbScore.FilePath, dbPart.FileName, cancellationToken);

        this.logger.LogDebug("New file was saved.");
    }

    public async Task<IReadOnlyCollection<OutgoingScoreDto>> GetScoresAsync(CancellationToken cancellationToken)
    {
        var dbScores = await this.dbContext.Scores
            .Include(p => p.Composer)
            .Include(p => p.Arranger)
            .ToListAsync(cancellationToken);

        if (!dbScores.Any())
        {
            throw new NotFoundException("No scores found.", "No scores can be found in database.");
        }

        return this.mapper.Map<IReadOnlyCollection<OutgoingScoreDto>>(dbScores);
    }
}
