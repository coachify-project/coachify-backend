using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditTestSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateNum",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "CourseTitle",
                table: "Certificates",
                newName: "Title");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Enrollments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Certificates",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Certificates",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Certificates",
                newName: "CourseTitle");

            migrationBuilder.AddColumn<int>(
                name: "CertificateNum",
                table: "Certificates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "Certificates",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
