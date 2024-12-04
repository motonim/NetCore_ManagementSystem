using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "145c0029-5f74-4c22-bd70-d2fe2cfc5b7e", null, "Employee", "EMPLOYEE" },
                    { "3033f938-c243-4cb6-ac28-27874b32687f", null, "Administrator", "ADMINISTRATOR" },
                    { "df88a034-71a6-476e-b25e-08a9059d4b2f", null, "Supervisor", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8", 0, "35e4ec74-6dd8-4b2d-830a-e49221e8d4b5", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEEBsj4IHJYbBi7e17vSLtwU3x9m+DiTuvcEY0fwFgRrNyHruRoQ2PETzjG33T8aiZg==", null, false, "0da60a67-c82d-4375-8637-566e10d75916", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3033f938-c243-4cb6-ac28-27874b32687f", "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "145c0029-5f74-4c22-bd70-d2fe2cfc5b7e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df88a034-71a6-476e-b25e-08a9059d4b2f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3033f938-c243-4cb6-ac28-27874b32687f", "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3033f938-c243-4cb6-ac28-27874b32687f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8");
        }
    }
}
