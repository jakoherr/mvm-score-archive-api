using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Service.Parts;

public interface IPartsService
{
    Task<Result<IReadOnlyCollection<OutgoingPartDto>>> GetPartsAsync(CancellationToken cancellationToken);

    Task<int> AddPartAsync(IncommingPartDto incommingPartDto, CancellationToken cancellationToken);
}