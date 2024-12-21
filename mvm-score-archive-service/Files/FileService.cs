using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Numerics;
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
        string fileName = this.ConvertScoreName(scoreName);

        string folderPath = Path.Combine(this.fileBasePath, fileName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            this.logger.LogDebug("The folder was created: {FolderName}", folderPath);
        }

        return fileName;
    }

    private string ConvertScoreName(string scoreName)
    {
        string lowerCase = scoreName.ToLower();
        string sanitized = Regex.Replace(lowerCase, @"[^a-z0-9\s]", "");

        return sanitized.Replace(" ", "-");
    }
}
