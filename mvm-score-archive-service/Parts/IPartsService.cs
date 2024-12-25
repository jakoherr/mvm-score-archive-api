
namespace Mvm.Score.Archive.Service.Parts;

public interface IPartsService
{
    Task<IReadOnlyCollection<OutgoingPartDto>> GetPartsAsync(CancellationToken cancellationToken);

    Task<int> AddPartAsync(IncommingPartDto incommingPartDto, CancellationToken cancellationToken);
}