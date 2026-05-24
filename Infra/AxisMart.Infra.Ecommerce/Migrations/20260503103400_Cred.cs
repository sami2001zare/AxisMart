using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxisMart.Infra.Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class Cred : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credential_Users_UserId",
                table: "Credential");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credential",
                table: "Credential");

            migrationBuilder.RenameTable(
                name: "Credential",
                newName: "Credentials");

            migrationBuilder.RenameIndex(
                name: "IX_Credential_UserId",
                table: "Credentials",
                newName: "IX_Credentials_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credentials",
                table: "Credentials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_UserId",
                table: "Credentials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_UserId",
                table: "Credentials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credentials",
                table: "Credentials");

            migrationBuilder.RenameTable(
                name: "Credentials",
                newName: "Credential");

            migrationBuilder.RenameIndex(
                name: "IX_Credentials_UserId",
                table: "Credential",
                newName: "IX_Credential_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credential",
                table: "Credential",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Credential_Users_UserId",
                table: "Credential",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
