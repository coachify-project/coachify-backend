using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgress_Lessons_LessonId",
                table: "UserLessonProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgress_ProgressStatus_StatusId",
                table: "UserLessonProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgress_Modules_ModuleId",
                table: "UserModuleProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgress_ProgressStatus_StatusId",
                table: "UserModuleProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModuleProgress",
                table: "UserModuleProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLessonProgress",
                table: "UserLessonProgress");

            migrationBuilder.RenameTable(
                name: "UserModuleProgress",
                newName: "UserModuleProgresses");

            migrationBuilder.RenameTable(
                name: "UserLessonProgress",
                newName: "UserLessonProgresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgress_UserId_ModuleId",
                table: "UserModuleProgresses",
                newName: "IX_UserModuleProgresses_UserId_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgress_StatusId",
                table: "UserModuleProgresses",
                newName: "IX_UserModuleProgresses_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgress_ModuleId",
                table: "UserModuleProgresses",
                newName: "IX_UserModuleProgresses_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgress_UserId_LessonId",
                table: "UserLessonProgresses",
                newName: "IX_UserLessonProgresses_UserId_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgress_StatusId",
                table: "UserLessonProgresses",
                newName: "IX_UserLessonProgresses_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgress_LessonId",
                table: "UserLessonProgresses",
                newName: "IX_UserLessonProgresses_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModuleProgresses",
                table: "UserModuleProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLessonProgresses",
                table: "UserLessonProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgresses_Lessons_LessonId",
                table: "UserLessonProgresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatus_StatusId",
                table: "UserLessonProgresses",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgresses_Modules_ModuleId",
                table: "UserModuleProgresses",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatus_StatusId",
                table: "UserModuleProgresses",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgresses_Lessons_LessonId",
                table: "UserLessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatus_StatusId",
                table: "UserLessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgresses_Modules_ModuleId",
                table: "UserModuleProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatus_StatusId",
                table: "UserModuleProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModuleProgresses",
                table: "UserModuleProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLessonProgresses",
                table: "UserLessonProgresses");

            migrationBuilder.RenameTable(
                name: "UserModuleProgresses",
                newName: "UserModuleProgress");

            migrationBuilder.RenameTable(
                name: "UserLessonProgresses",
                newName: "UserLessonProgress");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgresses_UserId_ModuleId",
                table: "UserModuleProgress",
                newName: "IX_UserModuleProgress_UserId_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgresses_StatusId",
                table: "UserModuleProgress",
                newName: "IX_UserModuleProgress_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModuleProgresses_ModuleId",
                table: "UserModuleProgress",
                newName: "IX_UserModuleProgress_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgresses_UserId_LessonId",
                table: "UserLessonProgress",
                newName: "IX_UserLessonProgress_UserId_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgresses_StatusId",
                table: "UserLessonProgress",
                newName: "IX_UserLessonProgress_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLessonProgresses_LessonId",
                table: "UserLessonProgress",
                newName: "IX_UserLessonProgress_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModuleProgress",
                table: "UserModuleProgress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLessonProgress",
                table: "UserLessonProgress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgress_Lessons_LessonId",
                table: "UserLessonProgress",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgress_ProgressStatus_StatusId",
                table: "UserLessonProgress",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgress_Modules_ModuleId",
                table: "UserModuleProgress",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgress_ProgressStatus_StatusId",
                table: "UserModuleProgress",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
