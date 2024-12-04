using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c924b52-a7a7-469f-a092-20473d970a88", new DateOnly(1955, 12, 15), "Default first name", "Default last name", "AQAAAAIAAYagAAAAEDHaPpDxp50iWNhxZZoiV6Ss2zKMoC9j0+ofLbOG1zxeXvXYFyOJu/6hkOBdxtcoPg==", "6558c811-b811-4f9a-92d7-b16a57919735" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "35e4ec74-6dd8-4b2d-830a-e49221e8d4b5", "AQAAAAIAAYagAAAAEEBsj4IHJYbBi7e17vSLtwU3x9m+DiTuvcEY0fwFgRrNyHruRoQ2PETzjG33T8aiZg==", "0da60a67-c82d-4375-8637-566e10d75916" });
        }
    }
}
