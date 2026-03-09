using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourtCaseManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "Id", "CourtType", "CreatedAt", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "District", new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5030), true, "Surat", "Surat District Court" },
                    { 2, "District", new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5030), true, "Ahmedabad", "Ahmedabad District Court" },
                    { 3, "High Court", new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5030), true, "Ahmedabad", "Gujarat High Court" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(4950), null, "Admin" },
                    { 2, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(4950), null, "Judge" },
                    { 3, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(4950), null, "Lawyer" },
                    { 4, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(4950), null, "Clerk" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5010), "admin@court.com", "System Admin", true, "admin123", 1 },
                    { 2, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5010), "judge@court.com", "Justice A. Shah", true, "judge123", 2 },
                    { 3, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5010), "lawyer@court.com", "Adv. Rohan Mehta", true, "lawyer123", 3 },
                    { 4, new DateTime(2026, 3, 3, 16, 30, 51, 467, DateTimeKind.Utc).AddTicks(5010), "clerk@court.com", "Court Clerk Priya Patel", true, "clerk123", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
