using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMSystemITEMCRUD.Migrations
{
    public partial class InitializeDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ItemCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemImageTitle = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ItemPackageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ItemImageName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
