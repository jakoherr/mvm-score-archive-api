namespace Mvm.Score.Archive.Repository.DbEntities;

public class ScoreSheet
{
    public int Id { get; set; }

    public required string Instrument { get; set; }

    public int Part { get; set; }

    public required string Key { get; set; }

    public ICollection<ScoreSet> ScoreSets { get; set; } = null!;
}
