using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;

namespace Mvm.Score.Archive.Service.ComposersService;

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

    public async Task<int> AddComposerAsync(CancellationToken cancellationToken, IncomingComposerDto composerDto)
    {
        DbComposer dbComposer = this.mapper.Map<DbComposer>(composerDto);

        this.dbContext.Add(dbComposer);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogInformation("New composer added: {@Composer}", dbComposer);

        return dbComposer.Id;
    }

    public async Task DeleteComposerByIdAsync(int id, CancellationToken cancellationToken)
    {
        await this.dbContext.Composers
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<OutgoingComposerDto?> GetComposerAsync(int id, CancellationToken cancellationToken)
    {
        DbComposer? dbComposer = await this.dbContext.Composers
            .FirstOrDefaultAsync(composer => composer.Id == id, cancellationToken);

        return this.mapper.Map<OutgoingComposerDto?>(dbComposer);
    }

    public async Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken)
    {
        List<DbComposer> composers = await this.dbContext.Composers.ToListAsync(cancellationToken);

        return this.mapper.Map<IReadOnlyList<OutgoingComposerDto>>(composers);
    }
}
