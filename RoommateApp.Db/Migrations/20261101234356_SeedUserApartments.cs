using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoommateApp.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserApartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserApartments",
                columns: new[] { "ApartmentId", "UserId", "IsAdmin" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 1, 2, false },
                    { 1, 3, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserApartments",
                keyColumns: new[] { "ApartmentId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserApartments",
                keyColumns: new[] { "ApartmentId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserApartments",
                keyColumns: new[] { "ApartmentId", "UserId" },
                keyValues: new object[] { 1, 3 });
        }
    }
}
