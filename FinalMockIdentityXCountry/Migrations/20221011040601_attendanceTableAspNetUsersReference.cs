using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class attendanceTableAspNetUsersReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_ApplicationUserId",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Attendances",
                newName: "RunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_ApplicationUserId",
                table: "Attendances",
                newName: "IX_Attendances_RunnerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_CoachId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_CoachId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Attendances",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_RunnerId",
                table: "Attendances",
                newName: "IX_Attendances_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_ApplicationUserId",
                table: "Attendances",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
