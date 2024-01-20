using Microsoft.EntityFrameworkCore;

namespace Mvm.Score.Archive.Repository.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {        
    }
}
