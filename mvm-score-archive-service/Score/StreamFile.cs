namespace Mvm.Score.Archive.Service.Score;

public class StreamFile
{
    public StreamFile(string fileName, Stream stream)
    {
        this.FileName = fileName;
        this.Stream = stream;
    }

    public string FileName { get; }

    public Stream Stream { get; }
}
