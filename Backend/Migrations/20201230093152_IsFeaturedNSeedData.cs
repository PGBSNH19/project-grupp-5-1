using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class IsFeaturedNSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Description", "IsAvailable", "IsFeatured", "Name", "ProductCategoryId", "Stock" },
                values: new object[,]
                {
                    { 1, "Pair these Samsung Galaxy Buds True Wireless Earbuds with your device and go.", true, false, "SAMSUNG Galaxy Buds", 1, 65 },
                    { 2, "Stay on top of your fitness with this Apple Watch Series 3.", true, true, "Apple Watch Series 3 GPS", 1, 34 },
                    { 3, "Meet the 2nd generation Nest Mini, the speaker you control with your voice.", true, false, "Google Nest Mini (2nd Generation) - Charcoal", 1, 11 },
                    { 4, "Keep your favorite songs, photos, videos and games, thanks to 32GB of built-in memory.", true, false, "SAMSUNG Galaxy Tab A 8.0\" 32 GB", 1, 23 },
                    { 5, "At less than four pounds, this thin and light silver Chromebook laptop is easy to take from room to room or on the road.", true, true, "HP 14\" Pentium 4GB / 64GB Chromebook", 1, 43 },
                    { 6, "Enjoy all-day comfort and distraction-free music with the new Studio ANC.", false, false, "JLab Audio Studio ANC On-Ear", 1, 37 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FirstName", "LastName", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "Ahmad", "Yassin", "7c4a8d09ca3762af61e59520943dc26494f8941b", 1, "ayassin" },
                    { 2, "Andre", "Morad", "7c4a8d09ca3762af61e59520943dc26494f8941b", 1, "amorad" },
                    { 3, "Nor", "Shiervani", "7c4a8d09ca3762af61e59520943dc26494f8941b", 1, "nshiervani" },
                    { 4, "Irvin", "Perez", "7c4a8d09ca3762af61e59520943dc26494f8941b", 1, "iperez" },
                    { 5, "Micael", "Wolter", "7c4a8d09ca3762af61e59520943dc26494f8941b", 1, "mwolter" },
                    { 6, "Jim", "Bob", "7c4a8d09ca3762af61e59520943dc26494f8941b", 2, "customerjim" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Product");
        }
    }
}
