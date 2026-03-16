using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbScore
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string Subtitle { get; set; } = string.Empty;

    public int ComposerId { get; set; }

    public required DbComposer Composer { get; set; }

    public int? ArrangerId { get; set; }

    public DbArranger? Arranger { get; set; }

    public string Genre { get; set; } = string.Empty;

    public Orchestra Orchestra { get; set; }

    public string Publisher { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public ICollection<DbPart> Parts { get; set; } = new List<DbPart>();
}
