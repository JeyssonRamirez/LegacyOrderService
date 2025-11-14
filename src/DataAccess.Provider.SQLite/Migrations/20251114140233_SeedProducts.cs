using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Provider.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price", "RegistrationDate", "Status" },
                values: new object[,]
                {
                    { 1L, "Widget", 12.99m, new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(63), 0 },
                    { 2L, "Gadget", 15.49m, new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(67), 0 },
                    { 3L, "Doohickey", 8.75m, new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(69), 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
