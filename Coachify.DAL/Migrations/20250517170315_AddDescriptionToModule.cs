using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modules",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modules");
        }
    }
}
