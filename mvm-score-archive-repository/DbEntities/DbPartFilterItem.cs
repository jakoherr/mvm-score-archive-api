namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbPartFilterItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int PartFilterId { get; set; }

    public int PartId { get; set; }

    public required DbPart Part { get; set; }
}