using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updateddatabasetable11_28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.UpdateData(
                table: "WorkoutInformation",
                keyColumn: "RunnerId",
                keyValue: null,
                column: "RunnerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "WorkoutInformation",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "WorkoutInformation",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
