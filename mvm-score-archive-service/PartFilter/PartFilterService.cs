using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ErrorHandling;
using Mvm.Score.Archive.Service.ErrorHandling.ErrorDescriptions;

namespace Mvm.Score.Archive.Service.PartFilter;

public class PartFilterService : IPartFilterService
{
    private readonly ILogger<PartFilterService> logger;
    private readonly IMapper mapper;
    private readonly AppDbContext context;

    public PartFilterService(
        ILogger<PartFilterService> logger,
        IMapper mapper,
        AppDbContext context)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<Result<ICollection<PartFilterOutgoingDto>>> GetAllFiltersForUserAsync(Guid? userId, CancellationToken cancellationToken)
    {
        var dbPartFilters = await this.context.PartFilters
            .Where(x => x.CreatedByUserId == userId || x.IsGlobal == true)
            .Include(x => x.PartFilterItems)
            .ThenInclude(x => x.Part)
            .ToListAsync(cancellationToken);

        if (!dbPartFilters.Any())
        {
            return Result<ICollection<PartFilterOutgoingDto>>.Failure(PartFilterErrors.NoPartFilters);
        }

        return Result<ICollection<PartFilterOutgoingDto>>
            .Success(this.mapper.Map<List<PartFilterOutgoingDto>>(dbPartFilters));
    }

    public async Task<int> CreateNewFilterAsync(
        Guid? userId,
        string? userName,
        PartFilterIncomingDto partFilterIncomingDto,
        CancellationToken cancellationToken)
    {
        var dbFilter = this.mapper.Map<PartFilterIncomingDto, DbPartFilter>(partFilterIncomingDto);

        dbFilter.CreatedByUserId = userId;
        dbFilter.CreatedByUserName = userName;

        this.context.PartFilters.Add(dbFilter);
        await this.context.SaveChangesAsync(cancellationToken);

        return dbFilter.Id;
    }
}