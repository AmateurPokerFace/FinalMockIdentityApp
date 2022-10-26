using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class addedFieldDataWasLoggedToWorkoutInformationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DataWasLogged",
                table: "WorkoutInformation",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataWasLogged",
                table: "WorkoutInformation");
        }
    }
}
