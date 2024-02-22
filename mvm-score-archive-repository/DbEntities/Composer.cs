namespace Mvm.Score.Archive.Repository.DbEntities;

public class Composer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public ICollection<ScoreSet> ComposedScores { get; set; } = new List<ScoreSet>();

    public ICollection<ScoreSet> ArrangedScores { get; set; } = new List<ScoreSet>();
}
