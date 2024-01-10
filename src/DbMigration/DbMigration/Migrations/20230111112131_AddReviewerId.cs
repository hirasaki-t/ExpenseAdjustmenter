using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    public partial class AddReviewerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApproveHistories",
                type: "nvarchar(26)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(26)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "ApproveHistories",
                type: "nvarchar(26)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "ApproveHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApproveHistories",
                type: "nvarchar(26)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(26)");
        }
    }
}
