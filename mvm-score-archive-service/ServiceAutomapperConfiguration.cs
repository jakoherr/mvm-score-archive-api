using AutoMapper;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;
using Mvm.Score.Archive.Service.Parts;
using Mvm.Score.Archive.Service.Score;

namespace Mvm.Score.Archive.Service;

public class ServiceAutomapperConfiguration : Profile
{
    public ServiceAutomapperConfiguration()
    {
        this.CreateMap<IncomingComposerDto, DbComposer>();

        this.CreateMap<DbComposer, OutgoingComposerDto>();

        this.CreateMap<IncomingArrangerDto, DbArranger>();

        this.CreateMap<DbArranger, OutgoingArrangerDto>();

        this.CreateMap<IncomingScoreDto, DbScore>();

        this.CreateMap<DbScore, OutgoingScoreDto>();

        this.CreateMap<IncommingPartDto, DbPart>();

        this.CreateMap<DbPart, OutgoingPartDto>();

        this.CreateMap<DbPart, Part>()
            .ForMember(x => x.PartNumber, opt => opt.MapFrom(x => x.Part));
    }
}
