using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class addedtableRepliesToMessageBoardResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses");

            migrationBuilder.UpdateData(
                table: "MessageBoardResponses",
                keyColumn: "ResponderId",
                keyValue: null,
                column: "ResponderId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ResponderId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RepliesToMessageBoardResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReplierId = table.Column<int>(type: "int", nullable: false),
                    ReplierName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReplyDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reply = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageBoardResponseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepliesToMessageBoardResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepliesToMessageBoardResponse_MessageBoardResponses_MessageB~",
                        column: x => x.MessageBoardResponseId,
                        principalTable: "MessageBoardResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RepliesToMessageBoardResponse_MessageBoardResponseId",
                table: "RepliesToMessageBoardResponse",
                column: "MessageBoardResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses",
                column: "ResponderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses");

            migrationBuilder.DropTable(
                name: "RepliesToMessageBoardResponse");

            migrationBuilder.AlterColumn<string>(
                name: "ResponderId",
                table: "MessageBoardResponses",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_AspNetUsers_ResponderId",
                table: "MessageBoardResponses",
                column: "ResponderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
