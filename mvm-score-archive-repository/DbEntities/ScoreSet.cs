namespace Mvm.Score.Archive.Repository.DbEntities;

public class ScoreSet
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string Subtitle { get; set; } = string.Empty;

    public int ComposerId { get; set; }

    public Composer Composer { get; set; } = null!;

    public int? ArrangementId { get; set; }

    public Composer? Arrangement { get; set; } = null!;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    public Orchestra Orchestra { get; set; }

    public string Publisher { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<ScoreSheet> ScoreSheets { get; set; } = null!;
}
