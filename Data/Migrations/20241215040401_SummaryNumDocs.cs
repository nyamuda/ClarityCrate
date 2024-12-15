using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clarity_Crate.Migrations
{
    /// <inheritdoc />
    public partial class SummaryNumDocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumDocumentsSummarized",
                table: "Summary",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumDocumentsSummarized",
                table: "Summary");
        }
    }
}
