using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_EnrollmentStatus_StatusId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentStatus",
                table: "EnrollmentStatus");

            migrationBuilder.RenameTable(
                name: "EnrollmentStatus",
                newName: "EnrollmentStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentStatuses",
                table: "EnrollmentStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_EnrollmentStatuses_StatusId",
                table: "Enrollments",
                column: "StatusId",
                principalTable: "EnrollmentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_EnrollmentStatuses_StatusId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentStatuses",
                table: "EnrollmentStatuses");

            migrationBuilder.RenameTable(
                name: "EnrollmentStatuses",
                newName: "EnrollmentStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentStatus",
                table: "EnrollmentStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_EnrollmentStatus_StatusId",
                table: "Enrollments",
                column: "StatusId",
                principalTable: "EnrollmentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
