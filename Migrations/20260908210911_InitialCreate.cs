using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelipadManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Helicopters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TailNum = table.Column<int>(type: "INTEGER", nullable: false),
                    Usability = table.Column<string>(type: "TEXT", nullable: false),
                    FlightStatus = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helicopters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ManagerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelicopterShift",
                columns: table => new
                {
                    HelicoptersId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShiftsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelicopterShift", x => new { x.HelicoptersId, x.ShiftsId });
                    table.ForeignKey(
                        name: "FK_HelicopterShift_Helicopters_HelicoptersId",
                        column: x => x.HelicoptersId,
                        principalTable: "Helicopters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HelicopterShift_Shifts_ShiftsId",
                        column: x => x.ShiftsId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Desctiprion = table.Column<string>(type: "TEXT", nullable: false),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeliId = table.Column<int>(type: "INTEGER", nullable: false),
                    HelicopterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Helicopters_HelicopterId",
                        column: x => x.HelicopterId,
                        principalTable: "Helicopters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeliTaskId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_Tasks_HeliTaskId",
                        column: x => x.HeliTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ArmyNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsManager = table.Column<bool>(type: "INTEGER", nullable: false),
                    HeliTaskId = table.Column<int>(type: "INTEGER", nullable: true),
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubTaskId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffMembers_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffMembers_Tasks_HeliTaskId",
                        column: x => x.HeliTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelicopterShift_ShiftsId",
                table: "HelicopterShift",
                column: "ShiftsId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_HeliTaskId",
                table: "StaffMembers",
                column: "HeliTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_ShiftId",
                table: "StaffMembers",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_SubTaskId",
                table: "StaffMembers",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_HeliTaskId",
                table: "SubTasks",
                column: "HeliTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_HelicopterId",
                table: "Tasks",
                column: "HelicopterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ShiftId",
                table: "Tasks",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelicopterShift");

            migrationBuilder.DropTable(
                name: "StaffMembers");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Helicopters");

            migrationBuilder.DropTable(
                name: "Shifts");
        }
    }
}
