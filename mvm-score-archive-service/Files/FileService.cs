using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        string fileName = ReplaceSpacesWithHyphens(scoreName);

        string folderPath = Path.Combine(this.fileBasePath, fileName);

        if (Directory.Exists(folderPath))
        {
            return fileName;
        }

        Directory.CreateDirectory(folderPath);
        this.logger.LogDebug("The folder was created: {FolderName}", folderPath);

        return fileName;
    }

    public void DeleteFolderAndFiles(string filePath)
    {
        string folderPath = Path.Combine(this.fileBasePath, filePath);

        if (!Directory.Exists(folderPath))
        {
            return;
        }

        Directory.Delete(folderPath, true);
        this.logger.LogDebug("The folder and its files were deleted: {ScorePath}", filePath);
    }

    public async Task RenameAndStoreFileAsync(
        IFormFile file,
        string scorePath,
        string partName,
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(this.fileBasePath, scorePath, partName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream, cancellationToken);

        this.logger.LogDebug("Successfully stored file to: {Path}", filePath);
    }

    public async Task<Stream> ReadFileFromDiskAsync(
        string filePath,
        CancellationToken cancellationToken)
    {
        string fullPath = Path.Combine(this.fileBasePath, filePath);
        return new MemoryStream(await File.ReadAllBytesAsync(fullPath, cancellationToken));
    }

    private static string ReplaceSpacesWithHyphens(string inputString)
    {
        string lowerCase = inputString.ToLower();

        return Regex.Replace(lowerCase, @"[^a-z0-9\s]", string.Empty)
            .Replace(" ", "-");
    }
}
