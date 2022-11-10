using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedWorkoutInformationTableAddedNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pace",
                table: "WorkoutInformation");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "WorkoutInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "WorkoutInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seconds",
                table: "WorkoutInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "Seconds",
                table: "WorkoutInformation");

            migrationBuilder.AddColumn<double>(
                name: "Pace",
                table: "WorkoutInformation",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
