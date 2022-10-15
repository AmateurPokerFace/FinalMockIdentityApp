using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class __test_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_CoachId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_CoachId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Practices",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_CoachId",
                table: "Practices",
                column: "CoachId");

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
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices");

            migrationBuilder.DropIndex(
                name: "IX_Practices_CoachId",
                table: "Practices");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Practices");

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Attendances",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CoachId",
                table: "Attendances",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_CoachId",
                table: "Attendances",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
