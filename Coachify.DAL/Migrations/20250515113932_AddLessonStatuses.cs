using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonMaterials",
                table: "Lessons",
                newName: "LessonObjectives");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Lessons",
                newName: "Introduction");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Lessons",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonStatusStatusId",
                table: "Lessons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonStatusStatusId",
                table: "Lessons",
                column: "LessonStatusStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonStatuses_LessonStatusStatusId",
                table: "Lessons",
                column: "LessonStatusStatusId",
                principalTable: "LessonStatuses",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonStatuses_LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonStatusStatusId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "LessonObjectives",
                table: "Lessons",
                newName: "LessonMaterials");

            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Lessons",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Lessons",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);
        }
    }
}
