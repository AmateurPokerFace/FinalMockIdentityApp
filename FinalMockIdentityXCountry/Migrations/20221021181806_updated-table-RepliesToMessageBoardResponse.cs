using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedtableRepliesToMessageBoardResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplierId",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.DropColumn(
                name: "ReplierName",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.AddColumn<string>(
                name: "ReplyerId",
                table: "RepliesToMessageBoardResponse",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReplyerName",
                table: "RepliesToMessageBoardResponse",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RepliesToMessageBoardResponse_ReplyerId",
                table: "RepliesToMessageBoardResponse",
                column: "ReplyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepliesToMessageBoardResponse_AspNetUsers_ReplyerId",
                table: "RepliesToMessageBoardResponse",
                column: "ReplyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepliesToMessageBoardResponse_AspNetUsers_ReplyerId",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.DropIndex(
                name: "IX_RepliesToMessageBoardResponse_ReplyerId",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.DropColumn(
                name: "ReplyerId",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.DropColumn(
                name: "ReplyerName",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.AddColumn<int>(
                name: "ReplierId",
                table: "RepliesToMessageBoardResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReplierName",
                table: "RepliesToMessageBoardResponse",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
