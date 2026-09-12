using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelipadManager.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionsSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelicopterShift_Helicopters_HelicoptersId",
                table: "HelicopterShift");

            migrationBuilder.DropForeignKey(
                name: "FK_HelicopterShift_Shifts_ShiftsId",
                table: "HelicopterShift");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffMembers_Tasks_HeliTaskId",
                table: "StaffMembers");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_HeliTaskId",
                table: "StaffMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HelicopterShift",
                table: "HelicopterShift");

            migrationBuilder.DropColumn(
                name: "HeliTaskId",
                table: "StaffMembers");

            migrationBuilder.RenameTable(
                name: "HelicopterShift",
                newName: "ShiftHelicopters");

            migrationBuilder.RenameIndex(
                name: "IX_HelicopterShift_ShiftsId",
                table: "ShiftHelicopters",
                newName: "IX_ShiftHelicopters_ShiftsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftHelicopters",
                table: "ShiftHelicopters",
                columns: new[] { "HelicoptersId", "ShiftsId" });

            migrationBuilder.CreateTable(
                name: "HeliTaskStaffMembers",
                columns: table => new
                {
                    ExecutorsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeliTaskStaffMembers", x => new { x.ExecutorsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_HeliTaskStaffMembers_StaffMembers_ExecutorsId",
                        column: x => x.ExecutorsId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeliTaskStaffMembers_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeliTaskStaffMembers_TasksId",
                table: "HeliTaskStaffMembers",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftHelicopters_Helicopters_HelicoptersId",
                table: "ShiftHelicopters",
                column: "HelicoptersId",
                principalTable: "Helicopters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftHelicopters_Shifts_ShiftsId",
                table: "ShiftHelicopters",
                column: "ShiftsId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftHelicopters_Helicopters_HelicoptersId",
                table: "ShiftHelicopters");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftHelicopters_Shifts_ShiftsId",
                table: "ShiftHelicopters");

            migrationBuilder.DropTable(
                name: "HeliTaskStaffMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftHelicopters",
                table: "ShiftHelicopters");

            migrationBuilder.RenameTable(
                name: "ShiftHelicopters",
                newName: "HelicopterShift");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftHelicopters_ShiftsId",
                table: "HelicopterShift",
                newName: "IX_HelicopterShift_ShiftsId");

            migrationBuilder.AddColumn<int>(
                name: "HeliTaskId",
                table: "StaffMembers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HelicopterShift",
                table: "HelicopterShift",
                columns: new[] { "HelicoptersId", "ShiftsId" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_HeliTaskId",
                table: "StaffMembers",
                column: "HeliTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_HelicopterShift_Helicopters_HelicoptersId",
                table: "HelicopterShift",
                column: "HelicoptersId",
                principalTable: "Helicopters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HelicopterShift_Shifts_ShiftsId",
                table: "HelicopterShift",
                column: "ShiftsId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffMembers_Tasks_HeliTaskId",
                table: "StaffMembers",
                column: "HeliTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
