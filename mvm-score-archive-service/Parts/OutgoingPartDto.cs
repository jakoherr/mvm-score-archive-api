using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Service.Parts;

public record OutgoingPartDto(
    int id,
    string Instrument,
    int? Part,
    string Tuning,
    string Clef);
