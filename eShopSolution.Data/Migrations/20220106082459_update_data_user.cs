using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class update_data_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 15, 24, 58, 344, DateTimeKind.Local).AddTicks(5376),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 13, 0, 29, 591, DateTimeKind.Local).AddTicks(7094));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "64e867aa-213a-4bf5-b283-a923668e0113");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "12c229d3-4326-41bb-a114-3a19f9b912d2", "cuongnm@gmail.com", "Nguyen Manh", "Cuong", "cuongnm@gmail.com", "AQAAAAEAACcQAAAAEOskYsBktzYgQstX8E/N37lnqkfcWiaMBLTT4Lqqgyu2aKzaAa3M8wiBuisdejXzqA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 1, 6, 15, 24, 58, 362, DateTimeKind.Local).AddTicks(9702));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 13, 0, 29, 591, DateTimeKind.Local).AddTicks(7094),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 15, 24, 58, 344, DateTimeKind.Local).AddTicks(5376));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "eee08a09-db9c-4abc-b1b1-d51dfef79102");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "649de7bc-048e-4e25-9bea-2d7d823851e0", "tedu.international@gmail.com", "Toan", "Bach", "tedu.international@gmail.com", "AQAAAAEAACcQAAAAENrGSH6X+dU6O/YoRV3w6l9mNJY7P6X029Mov4fO6hsWHObYSPdrdDUoRDIbVWPIXA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 1, 6, 13, 0, 29, 607, DateTimeKind.Local).AddTicks(8064));
        }
    }
}
