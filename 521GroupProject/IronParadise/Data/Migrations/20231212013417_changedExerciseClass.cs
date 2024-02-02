using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronParadise.Migrations
{
    public partial class changedExerciseClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isStandard",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "Reps",
                table: "Exercise",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Reps",
                table: "Exercise",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "isStandard",
                table: "Exercise",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
