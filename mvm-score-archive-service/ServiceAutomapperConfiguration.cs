using AutoMapper;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;

namespace Mvm.Score.Archive.Service;

public class ServiceAutomapperConfiguration : Profile
{
    public ServiceAutomapperConfiguration()
    {
        this.CreateMap<IncomingComposerDto, DbComposer>();

        this.CreateMap<DbComposer, OutgoingComposerDto>();

        this.CreateMap<IncomingArrangerDto, DbArranger>();

        this.CreateMap<DbArranger, OutgoingArrangerDto>();
    }
}
