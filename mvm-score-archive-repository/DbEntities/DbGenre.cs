namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbGenre
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<DbScore> Score { get; set; } = new List<DbScore>();
}
