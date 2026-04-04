using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Service.Parts;

public class Part
{
    public string Instrument { get; set; } = string.Empty;

    public int? PartNumber { get; set; }

    public Tunings Tuning { get; set; }

    public Clef Clef { get; set; }

    public int SortOrder { get; set; }

    public string FileName { get; set; } = string.Empty;
}