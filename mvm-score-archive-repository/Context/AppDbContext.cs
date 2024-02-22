using Microsoft.EntityFrameworkCore;
using Mvm.Score.Archive.Repository.DbEntities;

namespace Mvm.Score.Archive.Repository.Context;

public sealed class AppDbContext : DbContext
{
    private const string PostgresNow = "NOW()";

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Composer> Composers => this.Set<Composer>();

    public DbSet<Genre> Genres => this.Set<Genre>();

    public DbSet<ScoreSet> ScoresSets => this.Set<ScoreSet>();

    public DbSet<ScoreSheet> ScoreSheets => this.Set<ScoreSheet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Composer>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Genre>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ScoreSet>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.CreatedAt).HasDefaultValueSql(PostgresNow);
            e.HasOne(e => e.Composer)
                .WithMany(e => e.ComposedScores)
                .HasForeignKey(e => e.ComposerId)
                .IsRequired();

            e.HasOne(e => e.Arrangement)
                .WithMany(e => e.ArrangedScores)
                .HasForeignKey(e => e.ArrangementId)
                .IsRequired(false);

            e.HasOne(e => e.Genre)
                .WithMany(e => e.ScoreSets)
                .HasForeignKey(e => e.GenreId)
                .IsRequired();

            e.HasMany(e => e.ScoreSheets)
                .WithMany(e => e.ScoreSets);
        });

        modelBuilder.Entity<ScoreSheet>(e => e.HasKey(e => e.Id));
    }
}
