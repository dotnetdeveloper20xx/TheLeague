using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase7FacilityManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Venues_ClubId",
                table: "Venues");

            migrationBuilder.RenameColumn(
                name: "Facilities",
                table: "Venues",
                newName: "VirtualTourUrl");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Venues",
                newName: "TotalCapacity");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Venues",
                newName: "SpecialHours");

            migrationBuilder.AlterColumn<string>(
                name: "PostCode",
                table: "Venues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessInstructions",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalAmenities",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvanceBookingDays",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOnlineBooking",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AreaUnit",
                table: "Venues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingInstructions",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingSlotDuration",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CancellationFee",
                table: "Venues",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancellationNoticePeriod",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Venues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComplianceCertificates",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Venues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Venues",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyProcedures",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvacuationPlanUrl",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloorPlanUrl",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryUrls",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCatering",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasChangingRooms",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDefibrillator",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDisabledAccess",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFirstAid",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasLockers",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasShowers",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWifi",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpiryDate",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsurancePolicyNumber",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceProvider",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandlordContact",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandlordName",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInspectionDate",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaseEndDate",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaseStartDate",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxBookingDuration",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinBookingDuration",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRent",
                table: "Venues",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NearestHospital",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextInspectionDue",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Open24Hours",
                table: "Venues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OperatingHours",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnershipType",
                table: "Venues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpaces",
                table: "Venues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Venues",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Venues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalArea",
                table: "Venues",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Venues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Venues",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatThreeWords",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    MinimumCapacity = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Width = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Area = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    DimensionUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SurfaceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasLighting = table.Column<bool>(type: "bit", nullable: false),
                    HasHeating = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    IsIndoor = table.Column<bool>(type: "bit", nullable: false),
                    IsAccessible = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GalleryUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowOnlineBooking = table.Column<bool>(type: "bit", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    MinBookingDuration = table.Column<int>(type: "int", nullable: true),
                    MaxBookingDuration = table.Column<int>(type: "int", nullable: true),
                    DefaultBookingDuration = table.Column<int>(type: "int", nullable: true),
                    BookingSlotInterval = table.Column<int>(type: "int", nullable: true),
                    AdvanceBookingDays = table.Column<int>(type: "int", nullable: true),
                    MaxConcurrentBookings = table.Column<int>(type: "int", nullable: true),
                    BufferTimeBetweenBookings = table.Column<int>(type: "int", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MemberPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NonMemberPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PeakPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    OffPeakPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PricingType = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PriceIncludesVat = table.Column<bool>(type: "bit", nullable: false),
                    DefaultTaxRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AllowedMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    RequiredCertifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingRestrictions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaintenanceIntervalDays = table.Column<int>(type: "int", nullable: true),
                    MaintenanceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBookingsCount = table.Column<int>(type: "int", nullable: false),
                    TotalHoursBooked = table.Column<int>(type: "int", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LastBookedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentFacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SafetyGuidelines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facilities_Facilities_ParentFacilityId",
                        column: x => x.ParentFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facilities_TaxRates_DefaultTaxRateId",
                        column: x => x.DefaultTaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facilities_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueHolidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsRecurringAnnually = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueHolidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueHolidays_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueOperatingSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    PeakStartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    PeakEndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeasonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueOperatingSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueOperatingSchedules_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityAvailabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeasonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPeakTime = table.Column<bool>(type: "bit", nullable: false),
                    MembersOnly = table.Column<bool>(type: "bit", nullable: false),
                    AllowedMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityAvailabilities_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    ParticipantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGuestBooking = table.Column<bool>(type: "bit", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GuestPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckedInAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedInBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedOutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundProcessed = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrenceGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecurrenceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BookingSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBookings_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacilityMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignedTeam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PartsRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartsUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresInspection = table.Column<bool>(type: "bit", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PassedInspection = table.Column<bool>(type: "bit", nullable: false),
                    InspectionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrenceIntervalDays = table.Column<int>(type: "int", nullable: true),
                    NextScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentMaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPerformed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuesFound = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityMaintenances_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityMaintenances_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityMaintenances_FacilityMaintenances_ParentMaintenanceId",
                        column: x => x.ParentMaintenanceId,
                        principalTable: "FacilityMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityMaintenances_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityMaintenances_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacilityPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PricingType = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsMemberRate = table.Column<bool>(type: "bit", nullable: false),
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsJuniorRate = table.Column<bool>(type: "bit", nullable: false),
                    IsSeniorRate = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityPricings_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityPricings_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacilityBlockouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BlockoutType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsAllDay = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecurrenceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityBlockouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityBlockouts_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityBlockouts_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityBlockouts_FacilityMaintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "FacilityMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Venues_ClubId_Code",
                table: "Venues",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ClubId_Code",
                table: "Facilities",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_DefaultTaxRateId",
                table: "Facilities",
                column: "DefaultTaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ParentFacilityId",
                table: "Facilities",
                column: "ParentFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_VenueId",
                table: "Facilities",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityAvailabilities_FacilityId",
                table: "FacilityAvailabilities",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBlockouts_EventId",
                table: "FacilityBlockouts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBlockouts_FacilityId",
                table: "FacilityBlockouts",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBlockouts_MaintenanceId",
                table: "FacilityBlockouts",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_ClubId_BookingNumber",
                table: "FacilityBookings",
                columns: new[] { "ClubId", "BookingNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_EventId",
                table: "FacilityBookings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_FacilityId",
                table: "FacilityBookings",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_MemberId",
                table: "FacilityBookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_PaymentId",
                table: "FacilityBookings",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityBookings_SessionId",
                table: "FacilityBookings",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityMaintenances_ExpenseId",
                table: "FacilityMaintenances",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityMaintenances_FacilityId",
                table: "FacilityMaintenances",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityMaintenances_ParentMaintenanceId",
                table: "FacilityMaintenances",
                column: "ParentMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityMaintenances_PurchaseOrderId",
                table: "FacilityMaintenances",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityMaintenances_VendorId",
                table: "FacilityMaintenances",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityPricings_FacilityId",
                table: "FacilityPricings",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityPricings_MembershipTypeId",
                table: "FacilityPricings",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueHolidays_VenueId",
                table: "VenueHolidays",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueOperatingSchedules_VenueId",
                table: "VenueOperatingSchedules",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilityAvailabilities");

            migrationBuilder.DropTable(
                name: "FacilityBlockouts");

            migrationBuilder.DropTable(
                name: "FacilityBookings");

            migrationBuilder.DropTable(
                name: "FacilityPricings");

            migrationBuilder.DropTable(
                name: "VenueHolidays");

            migrationBuilder.DropTable(
                name: "VenueOperatingSchedules");

            migrationBuilder.DropTable(
                name: "FacilityMaintenances");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Venues_ClubId_Code",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AccessInstructions",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AdditionalAmenities",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AdvanceBookingDays",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AllowOnlineBooking",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "AreaUnit",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "BookingInstructions",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "BookingSlotDuration",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CancellationFee",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CancellationNoticePeriod",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ComplianceCertificates",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "EmergencyProcedures",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "EvacuationPlanUrl",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "FloorPlanUrl",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "GalleryUrls",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasCatering",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasChangingRooms",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasDefibrillator",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasDisabledAccess",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasFirstAid",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasLockers",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasShowers",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HasWifi",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "InsuranceExpiryDate",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "InsurancePolicyNumber",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "InsuranceProvider",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "LandlordContact",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "LandlordName",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "LastInspectionDate",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "LeaseEndDate",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "LeaseStartDate",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "MaxBookingDuration",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "MinBookingDuration",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "MonthlyRent",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "NearestHospital",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "NextInspectionDue",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Open24Hours",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "OperatingHours",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "OwnershipType",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ParkingSpaces",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "WhatThreeWords",
                table: "Venues");

            migrationBuilder.RenameColumn(
                name: "VirtualTourUrl",
                table: "Venues",
                newName: "Facilities");

            migrationBuilder.RenameColumn(
                name: "TotalCapacity",
                table: "Venues",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "SpecialHours",
                table: "Venues",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "PostCode",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Venues_ClubId",
                table: "Venues",
                column: "ClubId");
        }
    }
}
