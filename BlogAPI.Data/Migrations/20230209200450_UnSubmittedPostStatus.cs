using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Data.Migrations
{
    public partial class UnSubmittedPostStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PostStatus",
                columns: new[] { "PostStatusId", "Description" },
                values: new object[] { 4, "UnSubmitted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PostStatus",
                keyColumn: "PostStatusId",
                keyValue: 4);
        }
    }
}
