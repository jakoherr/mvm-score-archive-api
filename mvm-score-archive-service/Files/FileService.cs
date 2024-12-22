using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Mvm.Score.Archive.Service.Files;

public class FileService : IFileService
{
    private readonly ILogger<FileService> logger;
    private readonly string fileBasePath;

    public FileService(
        ILogger<FileService> logger,
        IOptions<FileSettings> fileSettingsConfiguration)
    {
        this.logger = logger;
        this.fileBasePath = fileSettingsConfiguration.Value.FilesBasePath;
    }

    public string CreateScoreFolder(string scoreName)
    {
        string fileName = ConvertScoreName(scoreName);

        string folderPath = Path.Combine(this.fileBasePath, fileName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            this.logger.LogDebug("The folder was created: {FolderName}", folderPath);
        }

        return fileName;
    }

    public async Task RenameAndStoreFileAsync(
        IFormFile file,
        string scorePath,
        string partName,
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(this.fileBasePath, scorePath, partName + ".pdf");

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream, cancellationToken);

        this.logger.LogDebug("Successfully stored file to: {Path}", filePath);
    }

    private static string ConvertScoreName(string scoreName)
    {
        string lowerCase = scoreName.ToLower();

        return Regex.Replace(lowerCase, @"[^a-z0-9\s]", "")
            .Replace(" ", "-");
    }
}
