using Mvm.Score.Archive.Repository.DbEnums;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;

namespace Mvm.Score.Archive.Service.Score;

public record OutgoingScoreDto(
    int Id,
    string Title,
    string Subtitle,
    OutgoingComposerDto Composer,
    OutgoingArrangerDto Arranger,
    string Genre,
    Orchestra Orchestra,
    string Publisher,
    DateTime CreatedAt);
