using Microsoft.AspNetCore.Http;

namespace Mvm.Score.Archive.Service.Files;

public interface IFileService
{
    string CreateScoreFolder(string scoreName);

    Task RenameAndStoreFileAsync(
        IFormFile file,
        string scorePath,
        string partName,
        CancellationToken cancellationToken);
}