using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class UpdateForeignKeysInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Coupon_CouponId",
                table: "Order");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalePrice",
                table: "ProductPrice",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CouponId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Coupon_CouponId",
                table: "Order",
                column: "CouponId",
                principalTable: "Coupon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Coupon_CouponId",
                table: "Order");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalePrice",
                table: "ProductPrice",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CouponId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Coupon_CouponId",
                table: "Order",
                column: "CouponId",
                principalTable: "Coupon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}