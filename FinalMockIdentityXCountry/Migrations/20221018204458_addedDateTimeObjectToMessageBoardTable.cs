using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class addedDateTimeObjectToMessageBoardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "MessageBoards",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MessageBoards",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropColumn(
                name: "PublishedDateTime",
                table: "MessageBoards");

            migrationBuilder.UpdateData(
                table: "MessageBoards",
                keyColumn: "CoachId",
                keyValue: null,
                column: "CoachId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "MessageBoards",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
