using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webhooks.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Webhook");

            migrationBuilder.CreateTable(
                name: "WebhookEvents",
                schema: "Webhook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WebhookName = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookSubscriptions",
                schema: "Webhook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    WebhookUri = table.Column<string>(type: "text", nullable: false),
                    Secret = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Webhooks = table.Column<string>(type: "text", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookSendAttempts",
                schema: "Webhook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WebhookEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: false, defaultValue: "{}"),
                    ResponseStatusCode = table.Column<int>(type: "integer", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookSendAttempts_WebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalSchema: "Webhook",
                        principalTable: "WebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvents_TenantId",
                schema: "Webhook",
                table: "WebhookEvents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookSendAttempts_TenantId",
                schema: "Webhook",
                table: "WebhookSendAttempts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookSendAttempts_WebhookEventId",
                schema: "Webhook",
                table: "WebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookSubscriptions_TenantId",
                schema: "Webhook",
                table: "WebhookSubscriptions",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhookSendAttempts",
                schema: "Webhook");

            migrationBuilder.DropTable(
                name: "WebhookSubscriptions",
                schema: "Webhook");

            migrationBuilder.DropTable(
                name: "WebhookEvents",
                schema: "Webhook");
        }
    }
}
