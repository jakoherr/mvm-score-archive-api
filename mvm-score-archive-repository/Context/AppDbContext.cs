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

    public DbSet<DbScore> Scores => this.Set<DbScore>();

    public DbSet<DbArranger> Arranges => this.Set<DbArranger>();

    public DbSet<DbPart> Parts => this.Set<DbPart>();

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

            e.HasMany(e => e.Parts)
                .WithMany();
        });

        modelBuilder.Entity<DbPart>(e =>
        {
            e.HasKey(e => e.Id);
            e.Ignore(e => e.FileName);
            e.HasMany<DbPart>()
                .WithOne()
                .HasForeignKey(x => x.FallbackPartId)
                .IsRequired(false);
        });

        // enums
        modelBuilder.HasPostgresEnum<Orchestra>(null, "orchestra");
        modelBuilder.HasPostgresEnum<Clef>(null, "clef");
        modelBuilder.HasPostgresEnum<Tunings>(null, "tunings");
    }
}
