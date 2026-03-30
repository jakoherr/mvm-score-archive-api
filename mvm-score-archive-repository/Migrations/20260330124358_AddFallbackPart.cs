using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFallbackPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fallback_part_id",
                table: "parts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_parts_fallback_part_id",
                table: "parts",
                column: "fallback_part_id");

            migrationBuilder.AddForeignKey(
                name: "fk_parts_parts_fallback_part_id",
                table: "parts",
                column: "fallback_part_id",
                principalTable: "parts",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_parts_parts_fallback_part_id",
                table: "parts");

            migrationBuilder.DropIndex(
                name: "ix_parts_fallback_part_id",
                table: "parts");

            migrationBuilder.DropColumn(
                name: "fallback_part_id",
                table: "parts");
        }
    }
}
