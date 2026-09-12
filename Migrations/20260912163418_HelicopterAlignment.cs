using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelipadManager.Migrations
{
    /// <inheritdoc />
    public partial class HelicopterAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Helicopters_HelicopterId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "HeliId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "HelicopterId",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Helicopters_HelicopterId",
                table: "Tasks",
                column: "HelicopterId",
                principalTable: "Helicopters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Helicopters_HelicopterId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "HelicopterId",
                table: "Tasks",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "HeliId",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Helicopters_HelicopterId",
                table: "Tasks",
                column: "HelicopterId",
                principalTable: "Helicopters",
                principalColumn: "Id");
        }
    }
}
