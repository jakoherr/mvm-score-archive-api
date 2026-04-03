namespace Mvm.Score.Archive.Service.Score;

public class StreamFile
{
    public StreamFile(string fileName, Stream stream, int? fileOrder = null)
    {
        this.FileName = fileName;
        this.Stream = stream;
        this.FileOrder = fileOrder;
    }

    public int? FileOrder { get; }

    public string FileName { get; }

    public Stream Stream { get; }
}
