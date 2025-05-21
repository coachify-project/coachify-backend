using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CourseStatuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Pending" },
                    { 3, "Published" },
                    { 4, "Rejected" },
                    { 5, "Not Started" },
                    { 6, "In progress" },
                    { 7, "Completed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CourseStatuses",
                keyColumn: "StatusId",
                keyValue: 7);
        }
    }
}
