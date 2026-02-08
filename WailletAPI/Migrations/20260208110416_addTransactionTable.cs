using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WailletAPI.Migrations
{
    /// <inheritdoc />
    public partial class addTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    tx_key = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_acc_key = table.Column<long>(type: "bigint", nullable: false),
                    to_acc_key = table.Column<long>(type: "bigint", nullable: false),
                    amount_from = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    amount_to = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    currency_from = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    currency_to = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    rate = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    initiator_user_key = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.tx_key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
