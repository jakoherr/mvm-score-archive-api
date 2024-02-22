﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mvm.Score.Archive.Repository.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240218195051_AddScoreSet")]
    partial class AddScoreSet
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.Composer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_composers");

                    b.ToTable("composers", (string)null);
                });

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Extention")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extention");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_genres");

                    b.ToTable("genres", (string)null);
                });

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.ScoreSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ArrangementId")
                        .HasColumnType("integer")
                        .HasColumnName("arrangement_id");

                    b.Property<int>("ComposerId")
                        .HasColumnType("integer")
                        .HasColumnName("composer_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer")
                        .HasColumnName("genre_id");

                    b.Property<int>("Orchestra")
                        .HasColumnType("integer")
                        .HasColumnName("orchestra");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("publisher");

                    b.Property<string>("Subtitle")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("subtitle");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_scores_sets");

                    b.HasIndex("ArrangementId")
                        .HasDatabaseName("ix_scores_sets_arrangement_id");

                    b.HasIndex("ComposerId")
                        .HasDatabaseName("ix_scores_sets_composer_id");

                    b.HasIndex("GenreId")
                        .HasDatabaseName("ix_scores_sets_genre_id");

                    b.ToTable("scores_sets", (string)null);
                });

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.ScoreSet", b =>
                {
                    b.HasOne("Mvm.Score.Archive.Repository.DbEntities.Composer", "Arrangement")
                        .WithMany("ArrangedScores")
                        .HasForeignKey("ArrangementId")
                        .HasConstraintName("fk_scores_sets_composers_arrangement_id");

                    b.HasOne("Mvm.Score.Archive.Repository.DbEntities.Composer", "Composer")
                        .WithMany("ComposedScores")
                        .HasForeignKey("ComposerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_scores_sets_composers_composer_id");

                    b.HasOne("Mvm.Score.Archive.Repository.DbEntities.Genre", "Genre")
                        .WithMany("ScoreSets")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_scores_sets_genres_genre_id");

                    b.Navigation("Arrangement");

                    b.Navigation("Composer");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.Composer", b =>
                {
                    b.Navigation("ArrangedScores");

                    b.Navigation("ComposedScores");
                });

            modelBuilder.Entity("Mvm.Score.Archive.Repository.DbEntities.Genre", b =>
                {
                    b.Navigation("ScoreSets");
                });
#pragma warning restore 612, 618
        }
    }
}