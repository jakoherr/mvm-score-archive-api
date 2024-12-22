using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:clef", "none,treble,bass,alto")
                .Annotation("Npgsql:Enum:orchestra", "unkown,vorstufe,juka,staka")
                .Annotation("Npgsql:Enum:tunings", "none,c,eb,bb,f,a")
                .OldAnnotation("Npgsql:Enum:orchestra", "unkown,vorstufe,juka,staka");

            migrationBuilder.CreateTable(
                name: "parts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    instrument = table.Column<string>(type: "text", nullable: false),
                    part = table.Column<int>(type: "integer", nullable: true),
                    tuning = table.Column<int>(type: "integer", nullable: false),
                    clef = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_part_db_score",
                columns: table => new
                {
                    db_score_id = table.Column<int>(type: "integer", nullable: false),
                    parts_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_db_part_db_score", x => new { x.db_score_id, x.parts_id });
                    table.ForeignKey(
                        name: "fk_db_part_db_score_parts_parts_id",
                        column: x => x.parts_id,
                        principalTable: "parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_db_part_db_score_scores_db_score_id",
                        column: x => x.db_score_id,
                        principalTable: "scores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_db_part_db_score_parts_id",
                table: "db_part_db_score",
                column: "parts_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_part_db_score");

            migrationBuilder.DropTable(
                name: "parts");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:orchestra", "unkown,vorstufe,juka,staka")
                .OldAnnotation("Npgsql:Enum:clef", "none,treble,bass,alto")
                .OldAnnotation("Npgsql:Enum:orchestra", "unkown,vorstufe,juka,staka")
                .OldAnnotation("Npgsql:Enum:tunings", "none,c,eb,bb,f,a");
        }
    }
}
