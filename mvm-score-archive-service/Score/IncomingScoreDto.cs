using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Service.Score;

public record IncomingScoreDto(
    string Title,
    string Subtitle,
    int ComposerId,
    int? ArrangerId,
    string Genre,
    Orchestra Orchestra,
    string Publisher);
