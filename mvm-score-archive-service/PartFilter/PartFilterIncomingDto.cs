namespace Mvm.Score.Archive.Service.PartFilter;

public record PartFilterIncomingDto(
    string Name,
    bool IsGlobal,
    IReadOnlyCollection<PartFilterItemIncomingDto> PartFilterItems);