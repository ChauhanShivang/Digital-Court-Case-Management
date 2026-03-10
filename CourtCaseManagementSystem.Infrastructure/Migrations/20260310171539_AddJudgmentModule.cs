using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtCaseManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJudgmentModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Judgments_CaseId",
                table: "Judgments");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 17, 15, 39, 456, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.CreateIndex(
                name: "IX_Judgments_CaseId",
                table: "Judgments",
                column: "CaseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Judgments_CaseId",
                table: "Judgments");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5700));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5700));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5700));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 16, 46, 31, 677, DateTimeKind.Utc).AddTicks(5710));

            migrationBuilder.CreateIndex(
                name: "IX_Judgments_CaseId",
                table: "Judgments",
                column: "CaseId");
        }
    }
}
