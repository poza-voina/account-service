using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InboxFixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxConsumed",
                table: "InboxConsumed");

            migrationBuilder.RenameTable(
                name: "InboxConsumed",
                newName: "inboxConsumed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inboxConsumed",
                table: "inboxConsumed",
                column: "messageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_inboxConsumed",
                table: "inboxConsumed");

            migrationBuilder.RenameTable(
                name: "inboxConsumed",
                newName: "InboxConsumed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxConsumed",
                table: "InboxConsumed",
                column: "messageId");
        }
    }
}
