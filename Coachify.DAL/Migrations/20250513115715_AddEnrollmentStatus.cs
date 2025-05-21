using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Enrollments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EnrollmentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EnrollmentStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Not Started" },
                    { 2, "In Progress" },
                    { 3, "Completed" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StatusId",
                table: "Enrollments",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_EnrollmentStatus_StatusId",
                table: "Enrollments",
                column: "StatusId",
                principalTable: "EnrollmentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_EnrollmentStatus_StatusId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "EnrollmentStatus");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StatusId",
                table: "Enrollments");

            migrationBuilder.DeleteData(
                table: "ModuleStatuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ModuleStatuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ModuleStatuses",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ModuleStatuses",
                keyColumn: "StatusId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Enrollments");

            migrationBuilder.InsertData(
                table: "CourseStatuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 5, "Not Started" },
                    { 6, "In progress" },
                    { 7, "Completed" }
                });
        }
    }
}
