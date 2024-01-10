using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    public partial class AddAllSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX85HADQ7CP26YMW29Z37FR3");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX85HADQVF40KRB3JPQHQKNG");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[,]
                {
                    { "01GX88M2873P25GHVDB668NPDM", null, true, true, "その他" },
                    { "01GX88M28759SXJ1KCZRKE0ADY", null, true, true, "宿泊費" },
                    { "01GX88M2875M6211CENX4TJRF0", null, true, false, "高速代" },
                    { "01GX88M2879M9HEKGH3EE053MR", null, true, false, "電車" },
                    { "01GX88M287HK8RFXWV7VXTFE52", null, true, true, "タクシー" },
                    { "01GX88M287PPF5F742159C5CHF", null, true, false, "バス" }
                });

            migrationBuilder.InsertData(
                table: "ExpenseTypes",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[,]
                {
                    { "01GX88M28715JQK7J4BCFF59NC", null, true, true, "事務用品購入費" },
                    { "01GX88M2872W49DXADT8GEBZVQ", null, true, true, "通信費" },
                    { "01GX88M2873NJPHJG9HMX18VPC", "1人当たりの金額が「5,001円（税込）」以上", true, true, "接待費" },
                    { "01GX88M28750KQ20F94Q3T1FHF", null, true, true, "書籍購入費" },
                    { "01GX88M2875M6P066TQXF4H4DV", null, true, true, "ハードウェア購入費" },
                    { "01GX88M287EH1GG5H8G34ST27N", null, true, true, "ソフトウェア購入費" },
                    { "01GX88M287G41P1XYT79EWJX6E", null, true, true, "福利厚生" },
                    { "01GX88M287REBZG5QRCVHKGFE1", "消耗品など", true, false, "雑貨" },
                    { "01GX88M287XD1V10SW0VN2XH76", "1人当たりの金額が「5,000円（税込）」以下", true, true, "会議費" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M2873P25GHVDB668NPDM");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M28759SXJ1KCZRKE0ADY");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M2875M6211CENX4TJRF0");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M2879M9HEKGH3EE053MR");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M287HK8RFXWV7VXTFE52");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "01GX88M287PPF5F742159C5CHF");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M28715JQK7J4BCFF59NC");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M2872W49DXADT8GEBZVQ");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M2873NJPHJG9HMX18VPC");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M28750KQ20F94Q3T1FHF");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M2875M6P066TQXF4H4DV");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M287EH1GG5H8G34ST27N");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M287G41P1XYT79EWJX6E");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M287REBZG5QRCVHKGFE1");

            migrationBuilder.DeleteData(
                table: "ExpenseTypes",
                keyColumn: "Id",
                keyValue: "01GX88M287XD1V10SW0VN2XH76");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[] { "01GX85HADQ7CP26YMW29Z37FR3", null, true, false, "電車" });

            migrationBuilder.InsertData(
                table: "ExpenseTypes",
                columns: new[] { "Id", "Details", "IsActive", "IsReceipt", "Name" },
                values: new object[] { "01GX85HADQVF40KRB3JPQHQKNG", "1人当たりの金額が「5,000円（税込）」以下", true, false, "会議費" });
        }
    }
}
