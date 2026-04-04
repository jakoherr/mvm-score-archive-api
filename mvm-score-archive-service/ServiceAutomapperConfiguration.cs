using AutoMapper;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;
using Mvm.Score.Archive.Service.PartFilter;
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

        this.CreateMap<DbPartFilter, PartFilterOutgoingDto>();

        this.CreateMap<DbPartFilterItem, PartFilterItemOutgoingDto>();

        this.CreateMap<PartFilterIncomingDto, DbPartFilter>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedByUserId, opt => opt.Ignore())
            .ForMember(x => x.CreatedByUserName, opt => opt.Ignore());

        this.CreateMap<PartFilterItemIncomingDto, DbPartFilterItem>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.PartFilterId, opt => opt.Ignore())
            .ForMember(x => x.Part, opt => opt.Ignore());
    }
}