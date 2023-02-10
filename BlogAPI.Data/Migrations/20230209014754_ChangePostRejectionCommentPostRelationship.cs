using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Data.Migrations
{
    public partial class ChangePostRejectionCommentPostRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostRejectionComment_PostRejectionCommentId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_PostRejectionCommentId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostRejectionCommentId",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "PostRejectionComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PostRejectionComment_Post_PostRejectionCommentId",
                table: "PostRejectionComment",
                column: "PostRejectionCommentId",
                principalTable: "Post",
                principalColumn: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostRejectionComment_Post_PostRejectionCommentId",
                table: "PostRejectionComment");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostRejectionComment");

            migrationBuilder.AddColumn<int>(
                name: "PostRejectionCommentId",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostRejectionCommentId",
                table: "Post",
                column: "PostRejectionCommentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostRejectionComment_PostRejectionCommentId",
                table: "Post",
                column: "PostRejectionCommentId",
                principalTable: "PostRejectionComment",
                principalColumn: "PostRejectionCommentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
