using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonStatuses_LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonStatuses_StatusId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_ModuleStatuses_StatusId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Lessons_LessonId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatus_StatusId",
                table: "UserLessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatus_StatusId",
                table: "UserModuleProgresses");

            migrationBuilder.DropTable(
                name: "LessonStatuses");

            migrationBuilder.DropTable(
                name: "ModuleStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LessonId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Modules_StatusId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_StatusId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgressStatus",
                table: "ProgressStatus");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Lessons");

            migrationBuilder.RenameTable(
                name: "ProgressStatus",
                newName: "ProgressStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgressStatuses",
                table: "ProgressStatuses",
                column: "StatusId");

            migrationBuilder.UpdateData(
                table: "ProgressStatuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "Name",
                value: "Not Started");

            migrationBuilder.UpdateData(
                table: "ProgressStatuses",
                keyColumn: "StatusId",
                keyValue: 3,
                column: "Name",
                value: "In Progress");

            migrationBuilder.InsertData(
                table: "ProgressStatuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[] { 1, "Draft" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatuses_StatusId",
                table: "UserLessonProgresses",
                column: "StatusId",
                principalTable: "ProgressStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatuses_StatusId",
                table: "UserModuleProgresses",
                column: "StatusId",
                principalTable: "ProgressStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatuses_StatusId",
                table: "UserLessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatuses_StatusId",
                table: "UserModuleProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgressStatuses",
                table: "ProgressStatuses");

            migrationBuilder.DeleteData(
                table: "ProgressStatuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "ProgressStatuses",
                newName: "ProgressStatus");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Tests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Modules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LessonStatusStatusId",
                table: "Lessons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Lessons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgressStatus",
                table: "ProgressStatus",
                column: "StatusId");

            migrationBuilder.CreateTable(
                name: "LessonStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "ModuleStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleStatuses", x => x.StatusId);
                });

            migrationBuilder.InsertData(
                table: "LessonStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Not Started" },
                    { 3, "In Progress" },
                    { 4, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "ModuleStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Not Started" },
                    { 3, "In progress" },
                    { 4, "Completed" }
                });

            migrationBuilder.UpdateData(
                table: "ProgressStatus",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "Name",
                value: "NotStarted");

            migrationBuilder.UpdateData(
                table: "ProgressStatus",
                keyColumn: "StatusId",
                keyValue: 3,
                column: "Name",
                value: "InProgress");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LessonId",
                table: "Tests",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_StatusId",
                table: "Modules",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonStatusStatusId",
                table: "Lessons",
                column: "LessonStatusStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_StatusId",
                table: "Lessons",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonStatuses_LessonStatusStatusId",
                table: "Lessons",
                column: "LessonStatusStatusId",
                principalTable: "LessonStatuses",
                principalColumn: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonStatuses_StatusId",
                table: "Lessons",
                column: "StatusId",
                principalTable: "LessonStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_ModuleStatuses_StatusId",
                table: "Modules",
                column: "StatusId",
                principalTable: "ModuleStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Lessons_LessonId",
                table: "Tests",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessonProgresses_ProgressStatus_StatusId",
                table: "UserLessonProgresses",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModuleProgresses_ProgressStatus_StatusId",
                table: "UserModuleProgresses",
                column: "StatusId",
                principalTable: "ProgressStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
