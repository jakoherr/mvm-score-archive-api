namespace Mvm.Score.Archive.Service.Composer;

public interface IComposerService
{
    Task<int> AddComposerAsync(IncomingComposerDto composerDto, CancellationToken cancellationToken);

    Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken);

    Task<OutgoingComposerDto> GetComposerByIdAsync(int id, CancellationToken cancellationToken);

    Task DeleteComposerByIdAsync(int id, CancellationToken cancellationToken);
}