namespace Mvm.Score.Archive.Service.ComposersService;

public interface IComposerService
{
    Task<int> AddComposerAsync(CancellationToken cancellationToken, IncomingComposerDto composerDto);

    Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken);

    Task<OutgoingComposerDto?> GetComposerAsync(int id, CancellationToken cancellationToken);

    Task DeleteComposerByIdAsync(int id, CancellationToken cancellationToken);
}