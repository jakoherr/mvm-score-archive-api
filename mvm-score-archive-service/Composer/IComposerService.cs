namespace Mvm.Score.Archive.Service.Composer;

public interface IComposerService
{
    Task<int> AddComposerAsync(CancellationToken cancellationToken, IncomingComposerDto composerDto);

    Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken);

    Task<OutgoingComposerDto?> GetComposerAsync(int id, CancellationToken cancellationToken);

    Task DeleteComposerByIdAsync(int id, CancellationToken cancellationToken);
}