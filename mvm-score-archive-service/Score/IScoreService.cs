
using Microsoft.AspNetCore.Http;

namespace Mvm.Score.Archive.Service.Score;

public interface IScoreService
{
    Task<int> AddScoreAsync(IncomingScoreDto incomingScoreDto, CancellationToken cancellationToken);

    Task AddScoreFileAsync(
        IFormFile file,
        int scoreId,
        int partId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<OutgoingScoreDto>> GetScoresAsync(CancellationToken cancellationToken);

    Task<StreamFile> ReadSingleScoreFileAsync(
        int scoreId,
        int partId,
        CancellationToken cancellationToken);
}