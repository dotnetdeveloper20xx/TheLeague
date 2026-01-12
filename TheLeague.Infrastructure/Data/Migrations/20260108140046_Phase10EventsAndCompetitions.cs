using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase10EventsAndCompetitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventTickets_TicketCode",
                table: "EventTickets");

            migrationBuilder.DropIndex(
                name: "IX_Events_ClubId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventRSVPs_EventId_MemberId",
                table: "EventRSVPs");

            migrationBuilder.RenameColumn(
                name: "UsedAt",
                table: "EventTickets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "EventTickets",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "EventTickets",
                newName: "FinalPrice");

            migrationBuilder.RenameColumn(
                name: "TicketCode",
                table: "EventTickets",
                newName: "TicketNumber");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "EventTickets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PurchasedAt",
                table: "EventTickets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "EventTickets",
                newName: "RefundRequested");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "EventTickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "AttendeeEmail",
                table: "EventTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendeeName",
                table: "EventTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendeePhone",
                table: "EventTickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "EventTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "EventTickets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "EventTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedInAt",
                table: "EventTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedInBy",
                table: "EventTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOutAt",
                table: "EventTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EventTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "EventTickets",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "EventTickets",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "EventTickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "EventTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedIn",
                table: "EventTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuest",
                table: "EventTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "EventTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "EventTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "EventTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "EventTickets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RefundAmount",
                table: "EventTickets",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RefundProcessed",
                table: "EventTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TicketType",
                table: "EventTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgeGroup",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AllowCheckIn",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowGuests",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOnlineRegistration",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowWaitlist",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AllowedMembershipTypes",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "Events",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CancellationFee",
                table: "Events",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancellationNoticeDays",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationPolicy",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledBy",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInClosesAt",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckInCode",
                table: "Events",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInOpensAt",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckInQRCode",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Events",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Events",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountCodes",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentUrls",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EarlyBirdDeadline",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EarlyBirdPrice",
                table: "Events",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeeId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryUrls",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GroupDiscount",
                table: "Events",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupDiscountMinSize",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSponsors",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllDay",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostponed",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LiveStreamUrl",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAge",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxGuestsPerRSVP",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxTicketsPerPerson",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingPoint",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MembersOnly",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MinAge",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinParticipants",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OccurrenceNumber",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizerName",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalStartDateTime",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentEventId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostponementReason",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublishedBy",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurrencePattern",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationCloseDate",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationOpenDate",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresApproval",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresRegistration",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOnWebsite",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstructions",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sponsors",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sport",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetGender",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditions",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TicketSalesStartDate",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalAttendees",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRegistrations",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRevenue",
                table: "Events",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitlistCount",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaitlistLimit",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "EventRSVPs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedInAt",
                table: "EventRSVPs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedInBy",
                table: "EventRSVPs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DietaryRequirements",
                table: "EventRSVPs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EventRSVPs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestNames",
                table: "EventRSVPs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestsCheckedIn",
                table: "EventRSVPs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedIn",
                table: "EventRSVPs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "EventRSVPs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EventRSVPs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "EventRSVPs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousResponse",
                table: "EventRSVPs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequirements",
                table: "EventRSVPs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EventRSVPs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaitlistPosition = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiscountReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCheckedIn = table.Column<bool>(type: "bit", nullable: false),
                    CheckedInAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedInBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedOutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundProcessed = table.Column<bool>(type: "bit", nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactRelation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DietaryRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsentFormSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_FamilyMembers_FamilyMemberId",
                        column: x => x.FamilyMemberId,
                        principalTable: "FamilyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSeries_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speaker = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    AttendeeCount = table.Column<int>(type: "int", nullable: false),
                    RequiresSeparateRegistration = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSessions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSessions_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSessions_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    AgeGroup = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    TargetGender = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationOpenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrawDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Format = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumberOfRounds = table.Column<int>(type: "int", nullable: true),
                    MatchesPerRound = table.Column<int>(type: "int", nullable: true),
                    HomeAndAway = table.Column<bool>(type: "bit", nullable: false),
                    LegsPerMatch = table.Column<int>(type: "int", nullable: true),
                    IsTeamBased = table.Column<bool>(type: "bit", nullable: false),
                    MinTeams = table.Column<int>(type: "int", nullable: true),
                    MaxTeams = table.Column<int>(type: "int", nullable: true),
                    CurrentTeamCount = table.Column<int>(type: "int", nullable: false),
                    MinPlayersPerTeam = table.Column<int>(type: "int", nullable: true),
                    MaxPlayersPerTeam = table.Column<int>(type: "int", nullable: true),
                    RequiresRegistration = table.Column<bool>(type: "bit", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    EntryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TeamEntryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PlayerEntryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HasPrizes = table.Column<bool>(type: "bit", nullable: false),
                    PrizeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrizeMoney = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Prizes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsForWin = table.Column<int>(type: "int", nullable: false),
                    PointsForDraw = table.Column<int>(type: "int", nullable: false),
                    PointsForLoss = table.Column<int>(type: "int", nullable: false),
                    BonusPointRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TiebreakerRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    HalfTimeDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    AllowSubstitutes = table.Column<bool>(type: "bit", nullable: false),
                    MaxSubstitutionsPerMatch = table.Column<int>(type: "int", nullable: true),
                    OrganizerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HasSponsors = table.Column<bool>(type: "bit", nullable: false),
                    Sponsors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BannerUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShowOnWebsite = table.Column<bool>(type: "bit", nullable: false),
                    TotalMatches = table.Column<int>(type: "int", nullable: false),
                    CompletedMatches = table.Column<int>(type: "int", nullable: false),
                    TotalGoals = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Fees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Members_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    MatchCount = table.Column<int>(type: "int", nullable: false),
                    CompletedMatchCount = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionRounds_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SeedNumber = table.Column<int>(type: "int", nullable: true),
                    DrawPosition = table.Column<int>(type: "int", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CaptainId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CaptainName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EntryFeePaid = table.Column<bool>(type: "bit", nullable: false),
                    EntryFeeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Played = table.Column<int>(type: "int", nullable: false),
                    Won = table.Column<int>(type: "int", nullable: false),
                    Drawn = table.Column<int>(type: "int", nullable: false),
                    Lost = table.Column<int>(type: "int", nullable: false),
                    GoalsFor = table.Column<int>(type: "int", nullable: false),
                    GoalsAgainst = table.Column<int>(type: "int", nullable: false),
                    GoalDifference = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    BonusPoints = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    HomeVenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeColors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AwayColors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionTeams_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionTeams_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionTeams_Members_CaptainId",
                        column: x => x.CaptainId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionTeams_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionTeams_Venues_HomeVenueId",
                        column: x => x.HomeVenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SquadNumber = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    EligibilityNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Appearances = table.Column<int>(type: "int", nullable: false),
                    Goals = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    YellowCards = table.Column<int>(type: "int", nullable: false),
                    RedCards = table.Column<int>(type: "int", nullable: false),
                    MinutesPlayed = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipants_CompetitionTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipants_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionStandings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    PreviousPosition = table.Column<int>(type: "int", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Played = table.Column<int>(type: "int", nullable: false),
                    Won = table.Column<int>(type: "int", nullable: false),
                    Drawn = table.Column<int>(type: "int", nullable: false),
                    Lost = table.Column<int>(type: "int", nullable: false),
                    GoalsFor = table.Column<int>(type: "int", nullable: false),
                    GoalsAgainst = table.Column<int>(type: "int", nullable: false),
                    GoalDifference = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    BonusPoints = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Form = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RecentResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPromoted = table.Column<bool>(type: "bit", nullable: false),
                    IsRelegated = table.Column<bool>(type: "bit", nullable: false),
                    QualifiedForNext = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionStandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionStandings_CompetitionTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionStandings_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AwayTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LegNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScheduledDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<int>(type: "int", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: true),
                    AwayScore = table.Column<int>(type: "int", nullable: true),
                    HomeHalfTimeScore = table.Column<int>(type: "int", nullable: true),
                    AwayHalfTimeScore = table.Column<int>(type: "int", nullable: true),
                    HomeExtraTimeScore = table.Column<int>(type: "int", nullable: true),
                    AwayExtraTimeScore = table.Column<int>(type: "int", nullable: true),
                    HomePenaltyScore = table.Column<int>(type: "int", nullable: true),
                    AwayPenaltyScore = table.Column<int>(type: "int", nullable: true),
                    HomeAggregateScore = table.Column<int>(type: "int", nullable: true),
                    AwayAggregateScore = table.Column<int>(type: "int", nullable: true),
                    FirstLegMatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomePointsAwarded = table.Column<int>(type: "int", nullable: true),
                    AwayPointsAwarded = table.Column<int>(type: "int", nullable: true),
                    HomeBonusPoints = table.Column<int>(type: "int", nullable: true),
                    AwayBonusPoints = table.Column<int>(type: "int", nullable: true),
                    RefereeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RefereeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssistantReferees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthOfficial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Attendance = table.Column<int>(type: "int", nullable: true),
                    Weather = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PitchCondition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPostponed = table.Column<bool>(type: "bit", nullable: false),
                    OriginalDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostponementReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MatchReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighlightsUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhotosUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HomeResultConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    HomeConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HomeConfirmedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AwayResultConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    AwayConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AwayConfirmedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDisputed = table.Column<bool>(type: "bit", nullable: false),
                    DisputeReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisputeResolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_CompetitionRounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "CompetitionRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_CompetitionTeams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_CompetitionTeams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Matches_FirstLegMatchId",
                        column: x => x.FirstLegMatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Members_RefereeId",
                        column: x => x.RefereeId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false),
                    AdditionalMinutes = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssistByParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubstitutedForParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchEvents_CompetitionParticipants_AssistByParticipantId",
                        column: x => x.AssistByParticipantId,
                        principalTable: "CompetitionParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchEvents_CompetitionParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "CompetitionParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchEvents_CompetitionParticipants_SubstitutedForParticipantId",
                        column: x => x.SubstitutedForParticipantId,
                        principalTable: "CompetitionParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchEvents_CompetitionTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchEvents_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchLineups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsStarting = table.Column<bool>(type: "bit", nullable: false),
                    ShirtNumber = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PositionOrder = table.Column<int>(type: "int", nullable: true),
                    MinutesPlayed = table.Column<int>(type: "int", nullable: true),
                    SubbedOnMinute = table.Column<int>(type: "int", nullable: true),
                    SubbedOffMinute = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    IsManOfTheMatch = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchLineups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchLineups_CompetitionParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "CompetitionParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchLineups_CompetitionTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "CompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchLineups_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_ClubId_TicketNumber",
                table: "EventTickets",
                columns: new[] { "ClubId", "TicketNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubId_Code",
                table: "Events",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Events_FacilityId",
                table: "Events",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_FeeId",
                table: "Events",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ParentEventId",
                table: "Events",
                column: "ParentEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SeriesId",
                table: "Events",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRSVPs_ClubId",
                table: "EventRSVPs",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRSVPs_EventId_MemberId",
                table: "EventRSVPs",
                columns: new[] { "EventId", "MemberId" },
                unique: true,
                filter: "[MemberId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipants_MemberId",
                table: "CompetitionParticipants",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipants_TeamId_MemberId",
                table: "CompetitionParticipants",
                columns: new[] { "TeamId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRounds_CompetitionId",
                table: "CompetitionRounds",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_ClubId_Code",
                table: "Competitions",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_FeeId",
                table: "Competitions",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OrganizerId",
                table: "Competitions",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_SeasonId",
                table: "Competitions",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_VenueId",
                table: "Competitions",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionStandings_CompetitionId_TeamId",
                table: "CompetitionStandings",
                columns: new[] { "CompetitionId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionStandings_TeamId",
                table: "CompetitionStandings",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeams_CaptainId",
                table: "CompetitionTeams",
                column: "CaptainId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeams_ClubId",
                table: "CompetitionTeams",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeams_CompetitionId_Code",
                table: "CompetitionTeams",
                columns: new[] { "CompetitionId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeams_HomeVenueId",
                table: "CompetitionTeams",
                column: "HomeVenueId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeams_PaymentId",
                table: "CompetitionTeams",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_ClubId_RegistrationNumber",
                table: "EventRegistrations",
                columns: new[] { "ClubId", "RegistrationNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId",
                table: "EventRegistrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_FamilyMemberId",
                table: "EventRegistrations",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_InvoiceId",
                table: "EventRegistrations",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_MemberId",
                table: "EventRegistrations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_PaymentId",
                table: "EventRegistrations",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeries_ClubId",
                table: "EventSeries",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessions_EventId",
                table: "EventSessions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessions_FacilityId",
                table: "EventSessions",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessions_VenueId",
                table: "EventSessions",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId",
                table: "Matches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FacilityId",
                table: "Matches",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FirstLegMatchId",
                table: "Matches",
                column: "FirstLegMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RefereeId",
                table: "Matches",
                column: "RefereeId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RoundId",
                table: "Matches",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_VenueId",
                table: "Matches",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_AssistByParticipantId",
                table: "MatchEvents",
                column: "AssistByParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_MatchId",
                table: "MatchEvents",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_ParticipantId",
                table: "MatchEvents",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_SubstitutedForParticipantId",
                table: "MatchEvents",
                column: "SubstitutedForParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_TeamId",
                table: "MatchEvents",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchLineups_MatchId_ParticipantId",
                table: "MatchLineups",
                columns: new[] { "MatchId", "ParticipantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchLineups_ParticipantId",
                table: "MatchLineups",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchLineups_TeamId",
                table: "MatchLineups",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_ClubId",
                table: "Seasons",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventRSVPs_Clubs_ClubId",
                table: "EventRSVPs",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventSeries_SeriesId",
                table: "Events",
                column: "SeriesId",
                principalTable: "EventSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Events_ParentEventId",
                table: "Events",
                column: "ParentEventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Facilities_FacilityId",
                table: "Events",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Fees_FeeId",
                table: "Events",
                column: "FeeId",
                principalTable: "Fees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Members_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTickets_Clubs_ClubId",
                table: "EventTickets",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventRSVPs_Clubs_ClubId",
                table: "EventRSVPs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventSeries_SeriesId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Events_ParentEventId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Facilities_FacilityId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Fees_FeeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Members_OrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTickets_Clubs_ClubId",
                table: "EventTickets");

            migrationBuilder.DropTable(
                name: "CompetitionStandings");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "EventSeries");

            migrationBuilder.DropTable(
                name: "EventSessions");

            migrationBuilder.DropTable(
                name: "MatchEvents");

            migrationBuilder.DropTable(
                name: "MatchLineups");

            migrationBuilder.DropTable(
                name: "CompetitionParticipants");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "CompetitionRounds");

            migrationBuilder.DropTable(
                name: "CompetitionTeams");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_EventTickets_ClubId_TicketNumber",
                table: "EventTickets");

            migrationBuilder.DropIndex(
                name: "IX_Events_ClubId_Code",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FacilityId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FeeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizerId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ParentEventId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_SeriesId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventRSVPs_ClubId",
                table: "EventRSVPs");

            migrationBuilder.DropIndex(
                name: "IX_EventRSVPs_EventId_MemberId",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "AttendeeEmail",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "AttendeeName",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "AttendeePhone",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CheckedInAt",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CheckedInBy",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CheckedOutAt",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "IsCheckedIn",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "IsGuest",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "RefundProcessed",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AllowCheckIn",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AllowGuests",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AllowOnlineRegistration",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AllowWaitlist",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AllowedMembershipTypes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CancellationFee",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CancellationNoticeDays",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CancellationPolicy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckInClosesAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckInCode",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckInOpensAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckInQRCode",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DiscountCodes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DocumentUrls",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EarlyBirdDeadline",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EarlyBirdPrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GalleryUrls",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GroupDiscount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GroupDiscountMinSize",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "HasSponsors",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsAllDay",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsPostponed",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LiveStreamUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxAge",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxGuestsPerRSVP",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxTicketsPerPerson",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MeetingPoint",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MembersOnly",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinAge",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinParticipants",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OccurrenceNumber",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganizerName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OriginalStartDateTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParentEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PostponementReason",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PublishedBy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RecurrencePattern",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationCloseDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationOpenDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RequiresApproval",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RequiresRegistration",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ShowOnWebsite",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SkillLevel",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Sponsors",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TargetGender",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketSalesStartDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TotalAttendees",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TotalRegistrations",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "WaitlistCount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "WaitlistLimit",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CheckedInAt",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "CheckedInBy",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "DietaryRequirements",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "GuestNames",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "GuestsCheckedIn",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "IsCheckedIn",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "PreviousResponse",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "SpecialRequirements",
                table: "EventRSVPs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EventRSVPs");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "EventTickets",
                newName: "UsedAt");

            migrationBuilder.RenameColumn(
                name: "TicketNumber",
                table: "EventTickets",
                newName: "TicketCode");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "EventTickets",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "RefundRequested",
                table: "EventTickets",
                newName: "IsUsed");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "EventTickets",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "EventTickets",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EventTickets",
                newName: "PurchasedAt");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "EventTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "EventRSVPs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_TicketCode",
                table: "EventTickets",
                column: "TicketCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubId",
                table: "Events",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRSVPs_EventId_MemberId",
                table: "EventRSVPs",
                columns: new[] { "EventId", "MemberId" },
                unique: true);
        }
    }
}
