using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WailletAPI.Migrations
{
    /// <inheritdoc />
    public partial class fkUserToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "user_key",
                table: "Accounts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_user_key",
                table: "Accounts",
                column: "user_key",
                unique: true,
                filter: "crypto_flag = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_user_key_currency_code",
                table: "Accounts",
                columns: new[] { "user_key", "currency_code" },
                unique: true,
                filter: "crypto_flag = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_user_key",
                table: "Accounts",
                column: "user_key",
                principalTable: "Users",
                principalColumn: "UserKey",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_user_key",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_user_key",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_user_key_currency_code",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "user_key",
                table: "Accounts");
        }
    }
}
