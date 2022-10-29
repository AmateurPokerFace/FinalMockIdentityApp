using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedMessageBoardResponsesAndMessageBoardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "MessageBoards",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RespondersName",
                table: "MessageBoardResponses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "MessageBoards");

            migrationBuilder.DropColumn(
                name: "RespondersName",
                table: "MessageBoardResponses");
        }
    }
}
