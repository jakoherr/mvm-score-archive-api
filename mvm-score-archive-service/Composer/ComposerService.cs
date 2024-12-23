using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Exceptions;

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

    public async Task DeleteComposerByIdAsync(int id, CancellationToken cancellationToken)
    {
        int deletedComposers = await this.dbContext.Composers
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedComposers == 0)
        {
            throw new NotFoundException("Composer not found.", $"The composer with id {id} was not found in the database.");
        }

        this.logger.LogInformation("The composer with id {Id} has been deleted.", id);
    }

    public async Task<OutgoingComposerDto> GetComposerByIdAsync(int id, CancellationToken cancellationToken)
    {
        DbComposer dbComposer = await this.dbContext.Composers
            .FirstOrDefaultAsync(composer => composer.Id == id, cancellationToken)
            ?? throw new NotFoundException("Composer not found.", $"The composer with id {id} was not found in the database.");

        return this.mapper.Map<OutgoingComposerDto>(dbComposer);
    }

    public async Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken)
    {
        List<DbComposer> composers = await this.dbContext.Composers.ToListAsync(cancellationToken);

        if (!composers.Any())
        {
            throw new NotFoundException("Composers not found.", "No composers in database");
        }

        return this.mapper.Map<IReadOnlyList<OutgoingComposerDto>>(composers);
    }
}
