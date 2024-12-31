namespace Mvm.Score.Archive.Service.Score;

public record IncomingPartMerge(
    IReadOnlyDictionary<int, int> PartAmounts);
