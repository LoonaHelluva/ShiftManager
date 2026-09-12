using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelipadManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSubTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffMembers_SubTasks_SubTaskId",
                table: "StaffMembers");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_SubTaskId",
                table: "StaffMembers");

            migrationBuilder.DropColumn(
                name: "SubTaskId",
                table: "StaffMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubTaskId",
                table: "StaffMembers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeliTaskId = table.Column<int>(type: "INTEGER", nullable: true),
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_SubTaskId",
                table: "StaffMembers",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_HeliTaskId",
                table: "SubTasks",
                column: "HeliTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffMembers_SubTasks_SubTaskId",
                table: "StaffMembers",
                column: "SubTaskId",
                principalTable: "SubTasks",
                principalColumn: "Id");
        }
    }
}
