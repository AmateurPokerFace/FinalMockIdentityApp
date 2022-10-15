using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class messageBoardResponsesTableAspNetUsersReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RunnerId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_CoachId",
                table: "MessageBoardResponses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_CoachId",
                table: "MessageBoardResponses",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "MessageBoardResponses");
        }
    }
}
