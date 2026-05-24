using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxisMart.Infra.Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class JsonWebToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JsonWebToken_Users_UserId",
                table: "JsonWebToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JsonWebToken",
                table: "JsonWebToken");

            migrationBuilder.RenameTable(
                name: "JsonWebToken",
                newName: "JsonWebTokens");

            migrationBuilder.RenameIndex(
                name: "IX_JsonWebToken_UserId",
                table: "JsonWebTokens",
                newName: "IX_JsonWebTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JsonWebTokens",
                table: "JsonWebTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JsonWebTokens_Users_UserId",
                table: "JsonWebTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JsonWebTokens_Users_UserId",
                table: "JsonWebTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JsonWebTokens",
                table: "JsonWebTokens");

            migrationBuilder.RenameTable(
                name: "JsonWebTokens",
                newName: "JsonWebToken");

            migrationBuilder.RenameIndex(
                name: "IX_JsonWebTokens_UserId",
                table: "JsonWebToken",
                newName: "IX_JsonWebToken_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JsonWebToken",
                table: "JsonWebToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JsonWebToken_Users_UserId",
                table: "JsonWebToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
