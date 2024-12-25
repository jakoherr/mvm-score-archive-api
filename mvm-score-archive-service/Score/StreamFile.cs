namespace Mvm.Score.Archive.Service.Score;

public class StreamFile
{
    public string FileName { get; }

    public Stream Stream { get; }

    public StreamFile(string fileName, Stream stream)
    {
        this.FileName = fileName;
        this.Stream = stream;
    }
}
