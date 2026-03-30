using System.Text.RegularExpressions;
using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbPart
{
    public int Id { get; set; }

    public required string Instrument { get; set; }

    public int? Part { get; set; }

    public Tunings Tuning { get; set; }

    public Clef Clef { get; set; }

    public int SortOrder { get; set; }

    public int? FallbackPartId { get; set; }

    public string FileName
    {
        get { return $"{ReplaceSpacesWithHyphens(this.Instrument)}_{this.Part ?? 0}_{this.Tuning.ToString()}.pdf"; }
    }

    private static string ReplaceSpacesWithHyphens(string inputString) =>
        Regex.Replace(inputString, @"[^a-z0-9A-Z\s]", string.Empty)
            .Replace(" ", "-");
}
