using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clarity_Crate.Migrations
{
    /// <inheritdoc />
    public partial class FavoriteDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefinitionComment_Definition_DefinitionId",
                table: "DefinitionComment");

            migrationBuilder.DropIndex(
                name: "IX_DefinitionComment_DefinitionId",
                table: "DefinitionComment");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                table: "DefinitionComment");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Definition");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "DefinitionComment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DefinitionFavorite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionFavorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefinitionFavorite_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefinitionFavorite_Definition_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Definition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefinitionLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefinitionLike_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefinitionLike_Definition_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Definition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionComment_ItemId",
                table: "DefinitionComment",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionFavorite_ItemId",
                table: "DefinitionFavorite",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionFavorite_UserId",
                table: "DefinitionFavorite",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionLike_ItemId",
                table: "DefinitionLike",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionLike_UserId",
                table: "DefinitionLike",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefinitionComment_Definition_ItemId",
                table: "DefinitionComment",
                column: "ItemId",
                principalTable: "Definition",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefinitionComment_Definition_ItemId",
                table: "DefinitionComment");

            migrationBuilder.DropTable(
                name: "DefinitionFavorite");

            migrationBuilder.DropTable(
                name: "DefinitionLike");

            migrationBuilder.DropIndex(
                name: "IX_DefinitionComment_ItemId",
                table: "DefinitionComment");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "DefinitionComment");

            migrationBuilder.AddColumn<int>(
                name: "DefinitionId",
                table: "DefinitionComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Definition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionComment_DefinitionId",
                table: "DefinitionComment",
                column: "DefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefinitionComment_Definition_DefinitionId",
                table: "DefinitionComment",
                column: "DefinitionId",
                principalTable: "Definition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
