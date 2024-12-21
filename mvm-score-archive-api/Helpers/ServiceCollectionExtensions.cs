using Mvm.Score.Archive.Service.Files;

namespace Mvm.Score.Archive.Api.Helpers;

public static class ServiceCollectionExtensions
{
    public static void AddFileSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var fileSettings = configuration.GetRequiredSection(nameof(FileSettings));

        services.Configure<FileSettings>(fileSettings);
    }
}
