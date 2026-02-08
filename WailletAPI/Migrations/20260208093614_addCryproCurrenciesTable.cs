using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WailletAPI.Migrations
{
    /// <inheritdoc />
    public partial class addCryproCurrenciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoCurrencies",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCurrencies", x => x.code);
                });

            migrationBuilder.InsertData(
                table: "CryptoCurrencies",
                columns: new[] { "code", "name" },
                values: new object[,]
                {
                    { "BTC", "Bitcoin" },
                    { "ETH", "Ethereum" },
                    { "LTC", "Litecoin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoCurrencies");
        }
    }
}
