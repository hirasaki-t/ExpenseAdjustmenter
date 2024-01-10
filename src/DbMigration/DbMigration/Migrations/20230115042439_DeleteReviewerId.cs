using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    public partial class DeleteReviewerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "ApproveHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "ApproveHistories",
                type: "nvarchar(26)",
                nullable: true);
        }
    }
}
