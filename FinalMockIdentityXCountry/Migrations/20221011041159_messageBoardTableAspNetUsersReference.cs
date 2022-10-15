using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class messageBoardTableAspNetUsersReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "MessageBoards",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoards_CoachId",
                table: "MessageBoards",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoards_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "MessageBoards");
        }
    }
}
