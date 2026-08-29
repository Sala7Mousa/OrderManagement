using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "admin@example.com", "AQAAAAIAAYagAAAAECw/ZzlNJ/B6j1d7ugtm38fQsUxoGTixdLETl5wA3qM9YkF0fCiVOFvuTG2px1dNUQ==", "Admin" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "customer@example.com", "AQAAAAIAAYagAAAAEIOwbny7HNAAGVK0n/ZJwqWO2PaHR1QxDSZF2BeQisND6QpYH41JnOT5ftVUAQ7sSw==", "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        }
    }
}
