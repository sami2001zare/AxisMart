using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AxisMart.Infra.Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AdministratorSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { new Guid("019e760f-c583-70c3-adb6-8d91e2fa3422"), "سامان", "زارع", "09121039846" },
                    { new Guid("019e760f-c583-7dc7-a590-f6b2e09ab5ad"), "رسول", "طاهری", "09123456789" }
                });

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Email", "UserName" },
                values: new object[,]
                {
                    { new Guid("019e760f-c583-70c3-adb6-8d91e2fa3422"), "saman.zare@modare.ac", "s.zare" },
                    { new Guid("019e760f-c583-7dc7-a590-f6b2e09ab5ad"), "r.taheri@gmail.com", "r.taheri" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: new Guid("019e760f-c583-70c3-adb6-8d91e2fa3422"));

            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: new Guid("019e760f-c583-7dc7-a590-f6b2e09ab5ad"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019e760f-c583-70c3-adb6-8d91e2fa3422"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019e760f-c583-7dc7-a590-f6b2e09ab5ad"));
        }
    }
}
