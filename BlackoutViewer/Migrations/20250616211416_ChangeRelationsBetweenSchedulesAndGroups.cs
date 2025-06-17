using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackoutViewer.Migrations;

/// <inheritdoc />
public partial class ChangeRelationsBetweenSchedulesAndGroups : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GroupSchedule");

        migrationBuilder.AddColumn<int>(
            name: "GroupId",
            table: "Schedules",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Schedules_GroupId",
            table: "Schedules",
            column: "GroupId");

        migrationBuilder.AddForeignKey(
            name: "FK_Schedules_Groups_GroupId",
            table: "Schedules",
            column: "GroupId",
            principalTable: "Groups",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Schedules_Groups_GroupId",
            table: "Schedules");

        migrationBuilder.DropIndex(
            name: "IX_Schedules_GroupId",
            table: "Schedules");

        migrationBuilder.DropColumn(
            name: "GroupId",
            table: "Schedules");

        migrationBuilder.CreateTable(
            name: "GroupSchedule",
            columns: table => new
            {
                GroupsId = table.Column<int>(type: "integer", nullable: false),
                SchedulesId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroupSchedule", x => new { x.GroupsId, x.SchedulesId });
                table.ForeignKey(
                    name: "FK_GroupSchedule_Groups_GroupsId",
                    column: x => x.GroupsId,
                    principalTable: "Groups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GroupSchedule_Schedules_SchedulesId",
                    column: x => x.SchedulesId,
                    principalTable: "Schedules",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GroupSchedule_SchedulesId",
            table: "GroupSchedule",
            column: "SchedulesId");
    }
}
