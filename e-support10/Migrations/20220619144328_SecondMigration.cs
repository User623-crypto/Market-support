using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_support10.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Carts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Carts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SupplyOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplyOrderRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Warranty = table.Column<int>(nullable: true),
                    Total = table.Column<double>(nullable: false),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyOrderRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyOrderRows_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyOrderRows_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyOrderRows_SupplyOrders_SupplyOrderId",
                        column: x => x.SupplyOrderId,
                        principalTable: "SupplyOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyOrderRows_OrderId",
                table: "SupplyOrderRows",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyOrderRows_ProductId",
                table: "SupplyOrderRows",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyOrderRows_SupplyOrderId",
                table: "SupplyOrderRows",
                column: "SupplyOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "SupplyOrderRows");

            migrationBuilder.DropTable(
                name: "SupplyOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Carts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
