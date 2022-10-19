using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedMessageBoardResponsesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.RenameColumn(
                name: "RunnerResponse",
                table: "MessageBoardResponses",
                newName: "Response");

            migrationBuilder.RenameColumn(
                name: "CoachResponse",
                table: "MessageBoardResponses",
                newName: "ResponderId");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.RenameColumn(
                name: "Response",
                table: "MessageBoardResponses",
                newName: "RunnerResponse");

            migrationBuilder.RenameColumn(
                name: "ResponderId",
                table: "MessageBoardResponses",
                newName: "CoachResponse");

            migrationBuilder.UpdateData(
                table: "MessageBoardResponses",
                keyColumn: "RunnerId",
                keyValue: null,
                column: "RunnerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_CoachId",
                table: "MessageBoardResponses",
                column: "CoachId");

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
    }
}
