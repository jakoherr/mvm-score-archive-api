using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:orchestra", "unkown,vorstufe,juka,staka");

            migrationBuilder.CreateTable(
                name: "arranges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_arranges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "composers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_composers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scores_sets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    subtitle = table.Column<string>(type: "text", nullable: false),
                    composer_id = table.Column<int>(type: "integer", nullable: false),
                    arranger_id = table.Column<int>(type: "integer", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    orchestra = table.Column<int>(type: "integer", nullable: false),
                    publisher = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scores_sets", x => x.id);
                    table.ForeignKey(
                        name: "fk_scores_sets_arranges_arranger_id",
                        column: x => x.arranger_id,
                        principalTable: "arranges",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_scores_sets_composers_composer_id",
                        column: x => x.composer_id,
                        principalTable: "composers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_scores_sets_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_scores_sets_arranger_id",
                table: "scores_sets",
                column: "arranger_id");

            migrationBuilder.CreateIndex(
                name: "ix_scores_sets_composer_id",
                table: "scores_sets",
                column: "composer_id");

            migrationBuilder.CreateIndex(
                name: "ix_scores_sets_genre_id",
                table: "scores_sets",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scores_sets");

            migrationBuilder.DropTable(
                name: "arranges");

            migrationBuilder.DropTable(
                name: "composers");

            migrationBuilder.DropTable(
                name: "genres");
        }
    }
}
