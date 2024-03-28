using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIMS2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    LotNumber = table.Column<string>(type: "TEXT", nullable: false),
                    LotName = table.Column<string>(type: "TEXT", nullable: false),
                    LotNotes = table.Column<string>(type: "TEXT", nullable: true),
                    TotalQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOnly = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.LotNumber);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAllocations",
                columns: table => new
                {
                    AllocationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerNumber = table.Column<string>(type: "TEXT", nullable: false),
                    QuantityUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    LotNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAllocations", x => x.AllocationID);
                    table.ForeignKey(
                        name: "FK_CustomerAllocations_Lots_LotNumber",
                        column: x => x.LotNumber,
                        principalTable: "Lots",
                        principalColumn: "LotNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAllocations_LotNumber",
                table: "CustomerAllocations",
                column: "LotNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAllocations");

            migrationBuilder.DropTable(
                name: "Lots");
        }
    }
}
