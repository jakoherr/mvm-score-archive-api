namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbPartFilter
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public string? CreatedByUserName { get; set; }

    public bool IsGlobal { get; set; }

    public ICollection<DbPartFilterItem> PartFilterItems { get; set; } = new List<DbPartFilterItem>();
}