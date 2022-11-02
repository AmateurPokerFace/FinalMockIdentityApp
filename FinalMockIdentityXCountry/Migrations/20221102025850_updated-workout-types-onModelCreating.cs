using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class updatedworkouttypesonModelCreating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "WorkoutName",
                value: "Easy Miles");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "WorkoutName",
                value: "Tempo Run");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "WorkoutName",
                value: "Long Run");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "WorkoutName",
                value: "Progression Run");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "WorkoutName",
                value: "Interval Run");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "WorkoutName",
                value: "Track Work");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "WorkoutName",
                value: "N/A");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "WorkoutName",
                value: "100-meters");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "WorkoutName",
                value: "200-meters");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "WorkoutName",
                value: "400-meters");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "WorkoutName",
                value: "800-meters");

            migrationBuilder.UpdateData(
                table: "WorkoutTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "WorkoutName",
                value: "1600-meters");
        }
    }
}
