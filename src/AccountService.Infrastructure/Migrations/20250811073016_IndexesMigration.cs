using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_bankAccountId",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_bankAccountId_createdAt",
                table: "transactions",
                columns: new[] { "bankAccountId", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_createdAt",
                table: "transactions",
                column: "createdAt")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_ownerId",
                table: "accounts",
                column: "ownerId")
                .Annotation("Npgsql:IndexMethod", "hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_bankAccountId_createdAt",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_createdAt",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_accounts_ownerId",
                table: "accounts");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_bankAccountId",
                table: "transactions",
                column: "bankAccountId");
        }
    }
}
