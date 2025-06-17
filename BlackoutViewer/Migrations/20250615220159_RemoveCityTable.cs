using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlackoutViewer.Migrations;

/// <inheritdoc />
public partial class RemoveCityTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Addresses_Cities_CityId",
            table: "Addresses");

        migrationBuilder.DropTable(
            name: "Cities");

        migrationBuilder.DropIndex(
            name: "IX_Addresses_CityId",
            table: "Addresses");

        migrationBuilder.DropColumn(
            name: "CityId",
            table: "Addresses");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CityId",
            table: "Addresses",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "Cities",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cities", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_CityId",
            table: "Addresses",
            column: "CityId");

        migrationBuilder.AddForeignKey(
            name: "FK_Addresses_Cities_CityId",
            table: "Addresses",
            column: "CityId",
            principalTable: "Cities",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
