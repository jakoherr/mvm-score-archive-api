using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Service.Composer;

public interface IComposerService
{
    Task<int> AddComposerAsync(IncomingComposerDto composerDto, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<OutgoingComposerDto>>> GetComposersAsync(CancellationToken cancellationToken);

    Task<Result<OutgoingComposerDto>> GetComposerByIdAsync(int id, CancellationToken cancellationToken);

    Task<Result<int>> DeleteComposerByIdAsync(int id, CancellationToken cancellationToken);
}