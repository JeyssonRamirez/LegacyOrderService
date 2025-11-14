using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Provider.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTableProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 17, 33, 982, DateTimeKind.Utc).AddTicks(5806));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 17, 33, 982, DateTimeKind.Utc).AddTicks(5809));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 17, 33, 982, DateTimeKind.Utc).AddTicks(5811));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(63));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 2, 33, 227, DateTimeKind.Utc).AddTicks(69));
        }
    }
}
