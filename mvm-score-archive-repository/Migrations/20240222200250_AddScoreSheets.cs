using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreSheets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "score_sheets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    instrument = table.Column<string>(type: "text", nullable: false),
                    part = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_score_sheets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "score_set_score_sheet",
                columns: table => new
                {
                    score_sets_id = table.Column<int>(type: "integer", nullable: false),
                    score_sheets_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_score_set_score_sheet", x => new { x.score_sets_id, x.score_sheets_id });
                    table.ForeignKey(
                        name: "fk_score_set_score_sheet_score_sheets_score_sheets_id",
                        column: x => x.score_sheets_id,
                        principalTable: "score_sheets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_score_set_score_sheet_scores_sets_score_sets_id",
                        column: x => x.score_sets_id,
                        principalTable: "scores_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_score_set_score_sheet_score_sheets_id",
                table: "score_set_score_sheet",
                column: "score_sheets_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "score_set_score_sheet");

            migrationBuilder.DropTable(
                name: "score_sheets");
        }
    }
}
