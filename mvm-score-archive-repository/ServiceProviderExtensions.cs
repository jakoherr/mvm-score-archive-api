using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvm.Score.Archive.Repository.Context;

namespace Mvm.Score.Archive.Repository;

public static class ServiceProviderExtensions
{
    public static void RunMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        database.Database.Migrate();
    }
}
