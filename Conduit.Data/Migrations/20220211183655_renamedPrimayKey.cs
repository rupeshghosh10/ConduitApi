using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class renamedPrimayKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowersId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowingId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "Followers",
                newName: "FollowingUserId");

            migrationBuilder.RenameColumn(
                name: "FollowersId",
                table: "Followers",
                newName: "FollowersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowingId",
                table: "Followers",
                newName: "IX_Followers_FollowingUserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Comments",
                newName: "AuthorUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                newName: "IX_Comments_AuthorUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Articles",
                newName: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AuthorUserId",
                table: "Comments",
                column: "AuthorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowersUserId",
                table: "Followers",
                column: "FollowersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowingUserId",
                table: "Followers",
                column: "FollowingUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowersUserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowingUserId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FollowingUserId",
                table: "Followers",
                newName: "FollowingId");

            migrationBuilder.RenameColumn(
                name: "FollowersUserId",
                table: "Followers",
                newName: "FollowersId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowingUserId",
                table: "Followers",
                newName: "IX_Followers_FollowingId");

            migrationBuilder.RenameColumn(
                name: "AuthorUserId",
                table: "Comments",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorUserId",
                table: "Comments",
                newName: "IX_Comments_AuthorId");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Articles",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowersId",
                table: "Followers",
                column: "FollowersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowingId",
                table: "Followers",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
