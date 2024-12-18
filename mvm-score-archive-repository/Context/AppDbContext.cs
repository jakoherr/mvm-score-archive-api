using Microsoft.EntityFrameworkCore;
using Mvm.Score.Archive.Repository.DbEntities;
using Mvm.Score.Archive.Repository.DbEnums;

namespace Mvm.Score.Archive.Repository.Context;

public sealed class AppDbContext : DbContext
{
    private const string PostgresNow = "NOW()";

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DbComposer> Composers => this.Set<DbComposer>();

    public DbSet<DbGenre> Genres => this.Set<DbGenre>();

    public DbSet<DbScore> ScoresSets => this.Set<DbScore>();

    public DbSet<DbArranger> Arranges => this.Set<DbArranger>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbComposer>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<DbGenre>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<DbScore>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.CreatedAt).HasDefaultValueSql(PostgresNow);
            e.HasOne(e => e.Composer)
                .WithMany(e => e.Scores)
                .HasForeignKey(e => e.ComposerId)
                .IsRequired();

            e.HasOne(e => e.Arranger)
                .WithMany(e => e.Scores)
                .HasForeignKey(e => e.ArrangerId)
                .IsRequired(false);

            e.HasOne(e => e.Genre)
                .WithMany(e => e.Score)
                .HasForeignKey(e => e.GenreId)
                .IsRequired();
        });

        // enums
        modelBuilder.HasPostgresEnum<Orchestra>(null, "orchestra");
    }
}
