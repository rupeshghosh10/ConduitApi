using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class ArticleTagTableCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Articles_ArticleId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Tags_TagId",
                table: "ArticleTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Articles_ArticleId",
                table: "ArticleTag",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Tags_TagId",
                table: "ArticleTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Articles_ArticleId",
                table: "ArticleTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTag_Tags_TagId",
                table: "ArticleTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Articles_ArticleId",
                table: "ArticleTag",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTag_Tags_TagId",
                table: "ArticleTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId");
        }
    }
}
