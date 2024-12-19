using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;

namespace Mvm.Score.Archive.Service.Arranger;

public class ArrangerService : IArrangerService
{
    private readonly ILogger<ArrangerService> logger;
    private readonly IMapper mapper;
    private readonly AppDbContext dbContext;

    public ArrangerService(
        ILogger<ArrangerService> logger,
        IMapper mapper,
        AppDbContext dbContext)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.dbContext = dbContext;
    }

    public async Task<int> AddArrangerAsync(IncomingArrangerDto arrangerDto, CancellationToken cancellationToken)
    {
        DbArranger dbArranger = this.mapper.Map<DbArranger>(arrangerDto);

        this.dbContext.Arranges.Add(dbArranger);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogInformation("New arranger added: {@Arranger}", dbArranger);

        return dbArranger.Id;
    }

    public async Task DeleteArrangerByIdAsync(int id, CancellationToken cancellationToken)
    {
        await this.dbContext.Arranges
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        this.logger.LogInformation("The arranger with id {Id} has been deleted.", id);
    }

    public async Task<OutgoingArrangerDto?> GetArrangerByIdAsync(int id, CancellationToken cancellationToken)
    {
        DbArranger? dbArranger = await this.dbContext.Arranges
            .FirstOrDefaultAsync(a => a.Id == id);

        return this.mapper.Map<OutgoingArrangerDto?>(dbArranger);
    }

    public async Task<IReadOnlyCollection<OutgoingArrangerDto>> GetArrangerAsync(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<DbArranger> arrangers = await this.dbContext.Arranges
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IReadOnlyCollection<OutgoingArrangerDto>>(arrangers);
    }
}
