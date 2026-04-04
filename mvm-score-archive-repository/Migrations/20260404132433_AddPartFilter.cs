using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mvm.Score.Archive.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddPartFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "part_filters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by_user_name = table.Column<string>(type: "text", nullable: true),
                    is_global = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_part_filters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "part_filter_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    part_filter_id = table.Column<int>(type: "integer", nullable: false),
                    part_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_part_filter_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_part_filter_items_part_filters_part_filter_id",
                        column: x => x.part_filter_id,
                        principalTable: "part_filters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_part_filter_items_parts_part_id",
                        column: x => x.part_id,
                        principalTable: "parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_part_filter_items_part_filter_id",
                table: "part_filter_items",
                column: "part_filter_id");

            migrationBuilder.CreateIndex(
                name: "ix_part_filter_items_part_id",
                table: "part_filter_items",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "ix_part_filters_created_by_user_id",
                table: "part_filters",
                column: "created_by_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part_filter_items");

            migrationBuilder.DropTable(
                name: "part_filters");
        }
    }
}