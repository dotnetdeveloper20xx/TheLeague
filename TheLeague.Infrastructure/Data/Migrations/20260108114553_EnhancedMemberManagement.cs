using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedMemberManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomFieldValues",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivationReason",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailOptIn",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoCardlessCustomerId",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramHandle",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MarketingOptIn",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedicalNotes",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberNumber",
                table: "Members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "OrientationCompleted",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrientationDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredContactMethod",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeData",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralSource",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferredByMemberId",
                table: "Members",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryEmergencyContactName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryEmergencyContactPhone",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryEmergencyContactRelation",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmsOptIn",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TermsAccepted",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TermsAcceptedDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterHandle",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WaiverAccepted",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "WaiverAcceptedDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaiverSignatureUrl",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunicationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableVariables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationTemplates_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placeholder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationRegex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsVisibleToMember = table.Column<bool>(type: "bit", nullable: false),
                    IsEditableByMember = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnRegistration = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnProfile = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldDefinitions_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberNotes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_ClubId_MemberNumber",
                table: "Members",
                columns: new[] { "ClubId", "MemberNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_ReferredByMemberId",
                table: "Members",
                column: "ReferredByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplates_ClubId_Category",
                table: "CommunicationTemplates",
                columns: new[] { "ClubId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldDefinitions_ClubId_Name",
                table: "CustomFieldDefinitions",
                columns: new[] { "ClubId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberNotes_ClubId_MemberId",
                table: "MemberNotes",
                columns: new[] { "ClubId", "MemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberNotes_MemberId",
                table: "MemberNotes",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_ReferredByMemberId",
                table: "Members",
                column: "ReferredByMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_ReferredByMemberId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "CommunicationTemplates");

            migrationBuilder.DropTable(
                name: "CustomFieldDefinitions");

            migrationBuilder.DropTable(
                name: "MemberNotes");

            migrationBuilder.DropIndex(
                name: "IX_Members_ClubId_MemberNumber",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_ReferredByMemberId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CustomFieldValues",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DeactivationReason",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EmailOptIn",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GoCardlessCustomerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "InstagramHandle",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MarketingOptIn",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MedicalNotes",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "OrientationCompleted",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "OrientationDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PreferredContactMethod",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PreferredLanguage",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "QRCodeData",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ReferralSource",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ReferredByMemberId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SecondaryEmergencyContactName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SecondaryEmergencyContactPhone",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SecondaryEmergencyContactRelation",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SmsOptIn",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TermsAccepted",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TermsAcceptedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TwitterHandle",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "WaiverAccepted",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "WaiverAcceptedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "WaiverSignatureUrl",
                table: "Members");
        }
    }
}
