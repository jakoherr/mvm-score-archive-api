using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbScore
{
    public int Id { get; set; }

    required public string Title { get; set; }

    public string Subtitle { get; set; } = string.Empty;

    public int ComposerId { get; set; }

    required public DbComposer Composer { get; set; }

    public int? ArrangerId { get; set; }

    public DbArranger? Arranger { get; set; }

    public int GenreId { get; set; }

    required public DbGenre Genre { get; set; }

    public Orchestra Orchestra { get; set; }

    public string Publisher { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
