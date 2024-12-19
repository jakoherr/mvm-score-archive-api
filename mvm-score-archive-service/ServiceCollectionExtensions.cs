using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvm.Score.Archive.Repository.Context;
using Mvm.Score.Archive.Service.Arranger;
using Mvm.Score.Archive.Service.Composer;

namespace Mvm.Score.Archive.Service;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectinString = configuration.GetConnectionString("ScoreArchiveContext");
        if (!string.IsNullOrEmpty(connectinString))
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(connectinString).UseSnakeCaseNamingConvention());
        }

        services.AddAutoMapper(c =>
        {
            c.AddProfile<ServiceAutomapperConfiguration>();
        });

        services.AddTransient<IComposerService, ComposerService>();
        services.AddTransient<IArrangerService, ArrangerService>();
    }
}
