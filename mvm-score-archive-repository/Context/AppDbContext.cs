using Microsoft.EntityFrameworkCore;
using Mvm.Score.Archive.Repository.DbEntities;

namespace Mvm.Score.Archive.Repository.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Composer> Composers => this.Set<Composer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Composer>(e =>
        {
            e.HasKey(e => e.Id);
        });
    }
}
