using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ErrorHandling;
using Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

namespace Mvm.Score.Archive.Service.Parts;

public class PartsService : IPartsService
{
    private readonly IMapper mapper;
    private readonly ILogger<PartsService> logger;
    private readonly AppDbContext dbContext;

    public PartsService(
        IMapper mapper,
        ILogger<PartsService> logger,
        AppDbContext dbContext)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyCollection<OutgoingPartDto>>> GetPartsAsync(CancellationToken cancellationToken)
    {
        var dbParts = await this.dbContext.Parts
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .ToListAsync(cancellationToken);

        if (!dbParts.Any())
        {
            return Result<IReadOnlyCollection<OutgoingPartDto>>.Failure(PartErrors.NoPartFound);
        }

        return Result<IReadOnlyCollection<OutgoingPartDto>>
            .Success(this.mapper.Map<IReadOnlyCollection<OutgoingPartDto>>(dbParts));
    }

    public async Task<int> AddPartAsync(IncommingPartDto incommingPartDto, CancellationToken cancellationToken)
    {
        var dbPart = this.mapper.Map<DbPart>(incommingPartDto);

        this.dbContext.Parts.Add(dbPart);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        this.logger.LogInformation("New part added: {@Part}", dbPart);

        return dbPart.Id;
    }
}
