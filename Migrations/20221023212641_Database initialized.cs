using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIM_Dashboard.Migrations
{
    public partial class Databaseinitialized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ProductLifecycleStatus = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    ProductShortDescription = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    ProductLongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductManager = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ProductCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResourceFileName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ResourceImageTitle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ItemStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemRetailPrice = table.Column<double>(type: "float", nullable: true),
                    ItemPackageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemQuantityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemPackageQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemEngineType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemServiceInterval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemBrandColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemBaseColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNutritionalFacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemFoodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemForceSend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResourceFileName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ResourceImageTitle = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemName",
                table: "Items",
                column: "ItemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductId",
                table: "Items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
