using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoommateApp.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddApartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardThreshold = table.Column<int>(type: "int", nullable: false),
                    ResetSchedule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Apartments",
                columns: new[] { "Id", "Name", "ResetSchedule", "RewardThreshold" },
                values: new object[] { 1, "Apartment 4B", "Sunday 12:00 AM", 5000 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartments");
        }
    }
}
