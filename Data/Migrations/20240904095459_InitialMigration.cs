using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clarity_Crate.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermLevel");

            migrationBuilder.DropIndex(
                name: "IX_Definition_TermId",
                table: "Definition");

            migrationBuilder.CreateTable(
                name: "DefinitionLevel",
                columns: table => new
                {
                    DefinitionsId = table.Column<int>(type: "int", nullable: false),
                    LevelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionLevel", x => new { x.DefinitionsId, x.LevelsId });
                    table.ForeignKey(
                        name: "FK_DefinitionLevel_Definition_DefinitionsId",
                        column: x => x.DefinitionsId,
                        principalTable: "Definition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefinitionLevel_Level_LevelsId",
                        column: x => x.LevelsId,
                        principalTable: "Level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definition_TermId",
                table: "Definition",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionLevel_LevelsId",
                table: "DefinitionLevel",
                column: "LevelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefinitionLevel");

            migrationBuilder.DropIndex(
                name: "IX_Definition_TermId",
                table: "Definition");

            migrationBuilder.CreateTable(
                name: "TermLevel",
                columns: table => new
                {
                    LevelsId = table.Column<int>(type: "int", nullable: false),
                    TermsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermLevel", x => new { x.LevelsId, x.TermsId });
                    table.ForeignKey(
                        name: "FK_TermLevel_Level_LevelsId",
                        column: x => x.LevelsId,
                        principalTable: "Level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TermLevel_Term_TermsId",
                        column: x => x.TermsId,
                        principalTable: "Term",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definition_TermId",
                table: "Definition",
                column: "TermId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermLevel_TermsId",
                table: "TermLevel",
                column: "TermsId");
        }
    }
}
