using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class TagTextIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tags_Text",
                table: "Tags",
                column: "Text",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Text",
                table: "Tags");
        }
    }
}
