using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9d19e8a1-bdea-4d17-8b16-1ebc8386d38a", "07ae7371-09c6-4e16-a5c0-8543b64b4db0", "User", "USER" },
                    { "df0256f4-4433-4355-8daa-891cef5c8ad9", "ecbd2617-94c3-439a-abf7-9f7fa27543b0", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d19e8a1-bdea-4d17-8b16-1ebc8386d38a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df0256f4-4433-4355-8daa-891cef5c8ad9");
        }
    }
}
