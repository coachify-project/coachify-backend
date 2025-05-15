using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixModuleStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "StatusName",
                value: "Not Started");

            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 3,
                column: "StatusName",
                value: "In Progress");

            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 4,
                column: "StatusName",
                value: "Completed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "StatusName",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 3,
                column: "StatusName",
                value: "Published");

            migrationBuilder.UpdateData(
                table: "LessonStatuses",
                keyColumn: "StatusId",
                keyValue: 4,
                column: "StatusName",
                value: "Rejected");
        }
    }
}
