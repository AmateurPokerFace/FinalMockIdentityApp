using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class removed_nullable_foreign_keys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices");

            migrationBuilder.UpdateData(
                table: "Practices",
                keyColumn: "CoachId",
                keyValue: null,
                column: "CoachId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "Practices",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "Practices",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "MessageBoards",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoards_AspNetUsers_CoachId",
                table: "MessageBoards",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
