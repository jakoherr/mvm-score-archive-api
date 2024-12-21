
namespace Mvm.Score.Archive.Service.Score;

public interface IScoreService
{
    Task<int> AddScoreAsync(IncomingScoreDto incomingScoreDto, CancellationToken cancellationToken);
}