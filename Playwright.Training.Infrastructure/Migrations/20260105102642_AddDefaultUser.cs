using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playwright.Training.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "first_name", "deleted", "last_name", "username" },
                values: new object[] { new Guid("5c33e35c-606e-4d8f-91ec-1eba45094043"), "daniel@training.com", "Daniel", false, "Training", "daniel-training" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("5c33e35c-606e-4d8f-91ec-1eba45094043"));
        }
    }
}
