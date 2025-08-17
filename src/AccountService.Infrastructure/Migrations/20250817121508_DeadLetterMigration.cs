using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeadLetterMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outboxMesages",
                table: "outboxMesages");

            migrationBuilder.RenameTable(
                name: "outboxMesages",
                newName: "outboxMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outboxMessages",
                table: "outboxMessages",
                column: "id");

            migrationBuilder.CreateTable(
                name: "inboxDeadLetter",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    eventType = table.Column<string>(type: "text", nullable: true),
                    payload = table.Column<string>(type: "text", nullable: true),
                    failedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    exceptionMessage = table.Column<string>(type: "text", nullable: true),
                    stackTrace = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inboxDeadLetter", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inboxDeadLetter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outboxMessages",
                table: "outboxMessages");

            migrationBuilder.RenameTable(
                name: "outboxMessages",
                newName: "outboxMesages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outboxMesages",
                table: "outboxMesages",
                column: "id");
        }
    }
}
