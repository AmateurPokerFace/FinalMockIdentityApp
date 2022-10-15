using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMockIdentityXCountry.Migrations
{
    public partial class removedCoachAndRunnerClassFromDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Coaches_CoachId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Runners_RunnerId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_Coaches_CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoardResponses_Runners_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoards_Coaches_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutInformation_Runners_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutInformation_RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoards_CoachId",
                table: "MessageBoards");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_CoachId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_RunnerId",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Runners",
                table: "Runners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coaches",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "WorkoutInformation");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "MessageBoards");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "MessageBoardResponses");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "RunnerId",
                table: "Attendances");

            migrationBuilder.RenameTable(
                name: "Runners",
                newName: "Runner");

            migrationBuilder.RenameTable(
                name: "Coaches",
                newName: "Coach");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Attendances",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Runner",
                table: "Runner",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coach",
                table: "Coach",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ApplicationUserId",
                table: "Attendances",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_ApplicationUserId",
                table: "Attendances",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_ApplicationUserId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ApplicationUserId",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Runner",
                table: "Runner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coach",
                table: "Coach");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Attendances");

            migrationBuilder.RenameTable(
                name: "Runner",
                newName: "Runners");

            migrationBuilder.RenameTable(
                name: "Coach",
                newName: "Coaches");

            migrationBuilder.AddColumn<int>(
                name: "RunnerId",
                table: "WorkoutInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "MessageBoards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "MessageBoardResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RunnerId",
                table: "MessageBoardResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RunnerId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Runners",
                table: "Runners",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coaches",
                table: "Coaches",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutInformation_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoards_CoachId",
                table: "MessageBoards",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_CoachId",
                table: "MessageBoardResponses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoardResponses_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CoachId",
                table: "Attendances",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_RunnerId",
                table: "Attendances",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Coaches_CoachId",
                table: "Attendances",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Runners_RunnerId",
                table: "Attendances",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_Coaches_CoachId",
                table: "MessageBoardResponses",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoardResponses_Runners_RunnerId",
                table: "MessageBoardResponses",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoards_Coaches_CoachId",
                table: "MessageBoards",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutInformation_Runners_RunnerId",
                table: "WorkoutInformation",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
