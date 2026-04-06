using Mvm.Score.Archive.Service.Parts;

namespace Mvm.Score.Archive.Service.PartFilter;

public record PartFilterItemOutgoingDto(
    int Id,
    int Quantity,
    OutgoingPartDto Part);