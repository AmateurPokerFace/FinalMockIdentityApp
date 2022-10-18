using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class __test_messageboard__ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "MessageBoards",
                newName: "MessageTitle");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "WorkoutInformation",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "Practices",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MessageBody",
                table: "MessageBoards",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "Attendances",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Practices_AspNetUsers_CoachId",
                table: "Practices");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "MessageBody",
                table: "MessageBoards");

            migrationBuilder.RenameColumn(
                name: "MessageTitle",
                table: "MessageBoards",
                newName: "Message");

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
                table: "Attendances",
                keyColumn: "RunnerId",
                keyValue: null,
                column: "RunnerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "Attendances",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_RunnerId",
                table: "Attendances",
                column: "RunnerId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_AspNetUsers_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
