using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mvm.Score.Archive.Repository.Context;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseNpgsql("User ID=postgres;Password=postgres;Server=localhost;Port=5432;Database=mvm-score-archive-api;Pooling=true;")
        .UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}