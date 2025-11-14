using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Provider.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTablereCreatedProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 28, 3, 888, DateTimeKind.Utc).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 28, 3, 888, DateTimeKind.Utc).AddTicks(6226));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 28, 3, 888, DateTimeKind.Utc).AddTicks(6228));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 24, 32, 63, DateTimeKind.Utc).AddTicks(7626));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 24, 32, 63, DateTimeKind.Utc).AddTicks(7629));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 24, 32, 63, DateTimeKind.Utc).AddTicks(7630));
        }
    }
}
