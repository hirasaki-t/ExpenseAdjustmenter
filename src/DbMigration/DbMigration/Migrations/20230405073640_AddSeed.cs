using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    public partial class AddSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[] { "01GX85HADQ7CP26YMW29Z37FR3", null, true, false, "電車" });

            migrationBuilder.InsertData(
                table: "ExpenseTypes",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[] { "01GX85HADQVF40KRB3JPQHQKNG", "1人当たりの金額が「5,000円（税込）」以下", true, false, "会議費" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX85HADQ7CP26YMW29Z37FR3");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX85HADQVF40KRB3JPQHQKNG");
        }
    }
}
