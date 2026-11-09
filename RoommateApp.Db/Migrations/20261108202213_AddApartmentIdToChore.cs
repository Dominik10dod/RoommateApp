using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoommateApp.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddApartmentIdToChore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "Chores",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 2,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 5,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 6,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 7,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 8,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 9,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 10,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 11,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 12,
                column: "ApartmentId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Chores_ApartmentId",
                table: "Chores",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Apartments_ApartmentId",
                table: "Chores",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Apartments_ApartmentId",
                table: "Chores");

            migrationBuilder.DropIndex(
                name: "IX_Chores_ApartmentId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "Chores");
        }
    }
}
