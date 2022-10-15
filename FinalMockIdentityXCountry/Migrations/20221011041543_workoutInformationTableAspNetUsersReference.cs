using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class workoutInformationTableAspNetUsersReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RunnerId",
                table: "WorkoutInformation",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutInformation_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutInformation_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "WorkoutInformation");
        }
    }
}
