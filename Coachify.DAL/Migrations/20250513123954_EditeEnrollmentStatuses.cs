using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditeEnrollmentStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EnrollmentStatuses",
                newName: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "EnrollmentStatuses",
                newName: "Id");
        }
    }
}
