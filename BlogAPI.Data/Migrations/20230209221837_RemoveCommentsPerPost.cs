using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Data.Migrations
{
    public partial class RemoveCommentsPerPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPerPost");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PostStatus",
                keyColumn: "PostStatusId",
                keyValue: 4,
                column: "Description",
                value: "Unsubmitted");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PostId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comment");

            migrationBuilder.CreateTable(
                name: "CommentPerPost",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPerPost", x => new { x.PostId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentPerPost_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentPerPost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "PostStatus",
                keyColumn: "PostStatusId",
                keyValue: 4,
                column: "Description",
                value: "UnSubmitted");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPerPost_CommentId",
                table: "CommentPerPost",
                column: "CommentId");
        }
    }
}
