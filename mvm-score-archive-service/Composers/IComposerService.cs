
namespace Mvm.Score.Archive.Service.Composers;

public interface IComposerService
{
    Task<int> AddComposerAsync(CancellationToken cancellationToken, IncomingComposerDto composerDto);

    Task<IReadOnlyList<OutgoingComposerDto>> GetComposersAsync(CancellationToken cancellationToken);
}