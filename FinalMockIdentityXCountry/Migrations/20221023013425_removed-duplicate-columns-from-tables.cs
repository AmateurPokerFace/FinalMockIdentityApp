using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class removedduplicatecolumnsfromtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkoutDateTime",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "ReplyerName",
                table: "RepliesToMessageBoardResponse");

            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "MessageBoards");

            migrationBuilder.DropColumn(
                name: "RespondersName",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "AttendanceDate",
                table: "Attendances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WorkoutDateTime",
                table: "WorkoutInformation",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReplyerName",
                table: "RepliesToMessageBoardResponse",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "MessageBoards",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RespondersName",
                table: "MessageBoardResponses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AttendanceDate",
                table: "Attendances",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
