namespace Mvm.Score.Archive.Service.Arranger;

public interface IArrangerService
{
    Task<int> AddArrangerAsync(IncomingArrangerDto arrangerDto, CancellationToken cancellationToken);

    Task DeleteArrangerByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<OutgoingArrangerDto>> GetArrangerAsync(CancellationToken cancellationToken);

    Task<OutgoingArrangerDto?> GetArrangerByIdAsync(int id, CancellationToken cancellationToken);
}