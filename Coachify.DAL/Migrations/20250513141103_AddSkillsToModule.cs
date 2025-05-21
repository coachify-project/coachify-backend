using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coachify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillsToModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "ModuleSkill",
                columns: table => new
                {
                    ModulesModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillsSkillId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleSkill", x => new { x.ModulesModuleId, x.SkillsSkillId });
                    table.ForeignKey(
                        name: "FK_ModuleSkill_Modules_ModulesModuleId",
                        column: x => x.ModulesModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleSkill_Skills_SkillsSkillId",
                        column: x => x.SkillsSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSkill_SkillsSkillId",
                table: "ModuleSkill",
                column: "SkillsSkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleSkill");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
