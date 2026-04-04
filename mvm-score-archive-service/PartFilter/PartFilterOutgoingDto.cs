namespace Mvm.Score.Archive.Service.PartFilter;

public record PartFilterOutgoingDto(
    int Id,
    string Name,
    Guid? CreatedByUserId,
    string? CreatedByUserName,
    IReadOnlyCollection<PartFilterItemOutgoingDto> PartFilterItems);