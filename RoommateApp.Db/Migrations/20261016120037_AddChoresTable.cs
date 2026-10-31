using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoommateApp.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddChoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chores",
                columns: new[] { "Id", "Category", "DueDateText", "Points", "Status", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "CLEANING", "Today", 15, "ToDo", "Vacuum living room rug & hallway", 1 },
                    { 2, "MAINTENANCE", "Tomorrow", 25, "ToDo", "Fix leaky master bath faucet", 2 },
                    { 3, "SHOPPING", "Oct 24", 10, "ToDo", "Buy groceries (milk, eggs, fruit)", 3 },
                    { 4, "CLEANING", "Oct 25", 5, "ToDo", "Wipe down all kitchen counters", 2 },
                    { 5, "CLEANING", "Oct 26", 5, "ToDo", "Dust bookshelves and tv stand", 1 },
                    { 6, "CLEANING", "Today", 20, "InProgress", "Deep clean kitchen oven & stove", 1 },
                    { 7, "MAINTENANCE", "Tomorrow", 10, "InProgress", "Replace hallway ceiling bulb", 2 },
                    { 8, "SHOPPING", "Oct 24", 10, "InProgress", "Pick up toilet paper and laundry soap", 3 },
                    { 9, "CLEANING", "Done · Oct 21", 10, "Done", "Take out trash & recycling bins", 3 },
                    { 10, "CLEANING", "Done · Oct 22", 15, "Done", "Mop the kitchen tile floors", 2 },
                    { 11, "SHOPPING", "Done · Oct 22", 5, "Done", "Restock dish soap and hand sanitizer", 2 },
                    { 12, "MAINTENANCE", "Done · Oct 23", 15, "Done", "Tighten loose kitchen cabinet hinge", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chores_UserId",
                table: "Chores",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chores");
        }
    }
}
