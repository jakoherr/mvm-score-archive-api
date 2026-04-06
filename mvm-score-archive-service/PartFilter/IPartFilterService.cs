using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Service.PartFilter;

public interface IPartFilterService
{
    Task<Result<ICollection<PartFilterOutgoingDto>>> GetAllFiltersForUserAsync(Guid? userId, CancellationToken cancellationToken);

    Task<int> CreateNewFilterAsync(
        Guid? userId,
        string? userName,
        PartFilterIncomingDto partFilterIncomingDto,
        CancellationToken cancellationToken);
}