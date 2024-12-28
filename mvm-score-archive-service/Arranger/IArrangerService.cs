using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Service.Arranger;

public interface IArrangerService
{
    Task<int> AddArrangerAsync(IncomingArrangerDto arrangerDto, CancellationToken cancellationToken);

    Task<Result<int>> DeleteArrangerByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OutgoingArrangerDto>>> GetArrangerAsync(CancellationToken cancellationToken);

    Task<Result<OutgoingArrangerDto>> GetArrangerByIdAsync(int id, CancellationToken cancellationToken);
}