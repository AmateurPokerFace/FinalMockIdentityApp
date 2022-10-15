using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class removedFieldWorkoutsAreAssignedToThePracticeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkoutsAreAssigned",
                table: "Practices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WorkoutsAreAssigned",
                table: "Practices",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
