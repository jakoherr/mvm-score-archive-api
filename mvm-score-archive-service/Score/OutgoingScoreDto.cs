using Mvm.Score.Archive.Repository.DbEnums;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;
using Mvm.Score.Archive.Service.Parts;

namespace Mvm.Score.Archive.Service.Score;

public record OutgoingScoreDto(
    int Id,
    string Title,
    string Subtitle,
    OutgoingComposerDto Composer,
    OutgoingArrangerDto Arranger,
    IReadOnlyCollection<OutgoingPartDto> Parts,
    string Genre,
    Orchestra Orchestra,
    string Publisher,
    DateTime CreatedAt);
