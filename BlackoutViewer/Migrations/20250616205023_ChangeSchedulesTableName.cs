using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackoutViewer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchedulesTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSchedule_schedules_SchedulesId",
                table: "GroupSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedules",
                table: "schedules");

            migrationBuilder.RenameTable(
                name: "schedules",
                newName: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSchedule_Schedules_SchedulesId",
                table: "GroupSchedule",
                column: "SchedulesId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSchedule_Schedules_SchedulesId",
                table: "GroupSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedules",
                table: "schedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSchedule_schedules_SchedulesId",
                table: "GroupSchedule",
                column: "SchedulesId",
                principalTable: "schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}