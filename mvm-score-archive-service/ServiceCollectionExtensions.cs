using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvm.Score.Archive.Repository.Context;

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
    }
}
