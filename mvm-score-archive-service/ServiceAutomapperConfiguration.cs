using AutoMapper;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.Composers;

namespace Mvm.Score.Archive.Service;

public class ServiceAutomapperConfiguration : Profile
{
    public ServiceAutomapperConfiguration()
    {
        this.CreateMap<IncomingComposerDto, Composer>();

        this.CreateMap<Composer, OutgoingComposerDto>();
    }
}
