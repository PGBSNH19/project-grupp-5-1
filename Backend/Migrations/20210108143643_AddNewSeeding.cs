using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Backend.Migrations
{
    public partial class AddNewSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupon",
                columns: new[] { "Id", "Code", "Description", "Discount", "Enabled", "EndDate", "StartDate" },
                values: new object[,]
                {
                    { 1, "Flash", "Get 25% off on all stocks we have.", 0.25m, true, new DateTime(2021, 1, 15, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(1048), new DateTime(2021, 1, 8, 15, 36, 42, 703, DateTimeKind.Local).AddTicks(4219) },
                    { 2, "Year2021", "You will get 10% discount on every product you buy..", 0.1m, true, new DateTime(2021, 1, 21, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(2930), new DateTime(2021, 1, 11, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(2909) }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 2, "Headphones" },
                    { 3, "Smart Watches" },
                    { 4, "Tablets" }
                });

            migrationBuilder.InsertData(
                table: "ProductImage",
                columns: new[] { "Id", "ImageName", "IsDefault", "ProductId" },
                values: new object[,]
                {
                    { 12, "06B.jpg", false, 6 },
                    { 11, "06A.jpg", true, 6 },
                    { 10, "05B.jpg", false, 5 },
                    { 9, "05A.jpg", true, 5 },
                    { 8, "04B.jpg", false, 4 },
                    { 7, "04A.jpg", true, 4 },
                    { 5, "03A.jpg", true, 3 },
                    { 4, "02B.jpg", false, 2 },
                    { 3, "02A.jpg", true, 2 },
                    { 2, "01B.jpg", false, 1 },
                    { 1, "01A.jpg", true, 1 },
                    { 6, "03B.jpg", false, 3 }
                });

            migrationBuilder.InsertData(
                table: "ProductPrice",
                columns: new[] { "Id", "DateChanged", "Price", "ProductId", "SalePrice" },
                values: new object[,]
                {
                    { 6, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(8222), 319m, 6, 250m },
                    { 1, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(5736), 1300m, 1, 900m },
                    { 2, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(7207), 2290m, 2, null },
                    { 3, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(8171), 329m, 3, null },
                    { 4, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(8206), 2490m, 4, null },
                    { 5, new DateTime(2021, 1, 8, 15, 36, 42, 706, DateTimeKind.Local).AddTicks(8210), 3790m, 5, 3200m }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Username",
                value: "akyassin");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductCategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductCategoryId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductCategoryId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductCategoryId",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupon",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupon",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductPrice",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Username",
                value: "ayassin");
        }
    }
}