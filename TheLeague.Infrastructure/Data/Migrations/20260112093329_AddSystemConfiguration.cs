using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigurationAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyChanged = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MockPaymentDelayMs = table.Column<int>(type: "int", nullable: false),
                    MockPaymentFailureRate = table.Column<double>(type: "float", nullable: false),
                    StripePublishableKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StripeSecretKeyEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StripeWebhookSecretEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MockEmailDelayMs = table.Column<int>(type: "int", nullable: false),
                    SendGridApiKeyEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultFromEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultFromName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceMode = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowNewRegistrations = table.Column<bool>(type: "bit", nullable: false),
                    EnableEmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationAuditLogs");

            migrationBuilder.DropTable(
                name: "SystemConfigurations");
        }
    }
}
