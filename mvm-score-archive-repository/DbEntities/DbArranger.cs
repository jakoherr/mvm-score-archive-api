namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbArranger
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public ICollection<DbScore> Scores { get; set; } = new List<DbScore>();
}
