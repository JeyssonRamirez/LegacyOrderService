using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Provider.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 58, 22, 324, DateTimeKind.Utc).AddTicks(9229));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 58, 22, 324, DateTimeKind.Utc).AddTicks(9233));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RegistrationDate",
                value: new DateTime(2025, 11, 14, 14, 58, 22, 324, DateTimeKind.Utc).AddTicks(9234));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

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
    }
}
