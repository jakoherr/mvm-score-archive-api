using Microsoft.AspNetCore.Http;
using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Service.Score;

public interface IScoreService
{
    Task<int> AddScoreAsync(IncomingScoreDto incomingScoreDto, CancellationToken cancellationToken);

    Task<Result<int>> AddScoreFileAsync(IFormFile file, int scoreId, int partId, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OutgoingScoreDto>>> GetScoresAsync(CancellationToken cancellationToken);

    Task<Result<StreamFile>> ReadSingleScoreFileAsync(int scoreId, int partId, CancellationToken cancellationToken);

    Task<Result<StreamFile>> ReadAllFilesAndMergeAsync(
        int scoreId,
        IncomingPartMerge partMerge,
        CancellationToken cancellationToken);

    Task AddRandomScores(int amount, CancellationToken cancellationToken);
}