
namespace Mvm.Score.Archive.Service.Composers;

public interface IComposerService
{
    Task<int> AddComposer(CancellationToken cancellationToken, ComposerDto composerDto);
}