using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ErrorHandling;
using Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

namespace Mvm.Score.Archive.Service.Composer;

public class ComposerService : IComposerService
{
    private readonly ILogger<ComposerService> logger;
    private readonly AppDbContext dbContext;
    private readonly IMapper mapper;

    public ComposerService(
        ILogger<ComposerService> logger,
        AppDbContext dbContext,
        IMapper mapper)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<int> AddComposerAsync(IncomingComposerDto composerDto, CancellationToken cancellationToken)
    {
        DbComposer dbComposer = this.mapper.Map<DbComposer>(composerDto);

        this.dbContext.Add(dbComposer);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogInformation("New composer added: {@Composer}", dbComposer);

        return dbComposer.Id;
    }

    public async Task<Result<int>> DeleteComposerByIdAsync(int id, CancellationToken cancellationToken)
    {
        int deletedComposers = await this.dbContext.Composers
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedComposers == 0)
        {
            return Result<int>.Failure(ComposerErrors.ComposerNotFound(id));
        }

        this.logger.LogInformation("The composer with id {Id} has been deleted.", id);

        return Result<int>.Success(deletedComposers);
    }

    public async Task<Result<OutgoingComposerDto>> GetComposerByIdAsync(int id, CancellationToken cancellationToken)
    {
        DbComposer? dbComposer = await this.dbContext.Composers
            .FirstOrDefaultAsync(composer => composer.Id == id, cancellationToken);

        if (dbComposer is null)
        {
            return Result<OutgoingComposerDto>.Failure(ComposerErrors.ComposerNotFound(id));
        }

        return Result<OutgoingComposerDto>
            .Success(this.mapper.Map<OutgoingComposerDto>(dbComposer));
    }

    public async Task<Result<IReadOnlyList<OutgoingComposerDto>>> GetComposersAsync(CancellationToken cancellationToken)
    {
        List<DbComposer> composers = await this.dbContext.Composers.ToListAsync(cancellationToken);

        if (!composers.Any())
        {
            return Result<IReadOnlyList<OutgoingComposerDto>>.Failure(ComposerErrors.NoComposersFound);
        }

        return Result<IReadOnlyList<OutgoingComposerDto>>
            .Success(this.mapper.Map<IReadOnlyList<OutgoingComposerDto>>(composers));
    }
}
