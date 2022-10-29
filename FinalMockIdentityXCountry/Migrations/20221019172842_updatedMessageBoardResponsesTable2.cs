using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedMessageBoardResponsesTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.AlterColumn<string>(
                name: "ResponderId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_ResponderId",
                table: "MessageBoardResponses",
                column: "ResponderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses",
                column: "ResponderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_ResponderId",
                table: "MessageBoardResponses");

            migrationBuilder.AlterColumn<string>(
                name: "ResponderId",
                table: "MessageBoardResponses",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RunnerId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
