using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Repository.DbEntities;

public class DbPart
{
    public int Id { get; set; }

    required public string Instrument { get; set; }

    public int? Part { get; set; }

    public Tunings Tuning { get; set; }

    public Clef Clef { get; set; }

    public string FileName
    {
        get { return $"{this.Instrument}_{this.Part ?? 0}_{this.Tuning.ToString()}"; }
    }
}
