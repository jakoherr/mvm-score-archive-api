using System.Reflection;

namespace Mvm.Score.Archive.Api.Helpers;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "TODO API",
                        Version = "v1",
                    });
            });
    }
}
