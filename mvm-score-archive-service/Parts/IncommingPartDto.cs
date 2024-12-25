using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Service.Parts;

public record IncommingPartDto(
    string Instrument,
    int? Part,
    Tunings Tuning,
    Clef Clef);
