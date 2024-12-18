using AutoMapper;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Service.ComposersService;

namespace Mvm.Score.Archive.Service;

public class ServiceAutomapperConfiguration : Profile
{
    public ServiceAutomapperConfiguration()
    {
        this.CreateMap<IncomingComposerDto, DbComposer>();

        this.CreateMap<DbComposer, OutgoingComposerDto>();
    }
}
