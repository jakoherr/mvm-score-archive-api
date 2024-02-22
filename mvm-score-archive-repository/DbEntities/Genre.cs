namespace Mvm.Score.Archive.Repository.DbEntities;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Extention { get; set; } = string.Empty;

    public ICollection<ScoreSet> ScoreSets { get; set; } = new List<ScoreSet>();
}
