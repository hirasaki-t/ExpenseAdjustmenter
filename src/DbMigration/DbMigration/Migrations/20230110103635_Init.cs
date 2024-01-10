using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReceipt = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReceipt = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    SubmissionMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptId = table.Column<string>(type: "nvarchar(26)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApproveHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    ExpenseId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApproveHistories_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SundryInformations",
                columns: table => new
                {
                    ExpenseId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    ExpenseTypeId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticipationNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SundryInformations", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_SundryInformations_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SundryInformations_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraveringInformations",
                columns: table => new
                {
                    ExpenseId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    StartSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraveringInformations", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_TraveringInformations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraveringInformations_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApproveHistories_ExpenseId",
                table: "ApproveHistories",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTypes_Name",
                table: "ExpenseTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SundryInformations_ExpenseTypeId",
                table: "SundryInformations",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TraveringInformations_CategoryId",
                table: "TraveringInformations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Mail",
                table: "Users",
                column: "Mail",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApproveHistories");

            migrationBuilder.DropTable(
                name: "SundryInformations");

            migrationBuilder.DropTable(
                name: "TraveringInformations");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
