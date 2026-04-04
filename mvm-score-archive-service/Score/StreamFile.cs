using Mvm.Score.Archive.Service.Parts;

namespace Mvm.Score.Archive.Service.Score;

public class StreamFile
{
    public StreamFile(Stream stream, Part partInformation)
    {
        this.Stream = stream;
        this.PartInformation = partInformation;
    }

    public Part PartInformation { get; }

    public Stream Stream { get; }
}
