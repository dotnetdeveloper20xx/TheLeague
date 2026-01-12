using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MembershipPlansAndPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Memberships",
                newName: "TotalFreezeDaysUsed");

            migrationBuilder.AddColumn<int>(
                name: "AccessType",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalMemberFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdminFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvanceBookingDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowAnnualPayment",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowDowngrade",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowFreeze",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowMonthlyPayment",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowQuarterlyPayment",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowUpgrade",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoRenewDefault",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableUntil",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BiannualFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancellationNoticeDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "MembershipTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "MembershipTypes",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrentMemberCount",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultBillingCycle",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DowngradeToIds",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EarlyCancellationFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExcludedFacilities",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FortnightlyFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FreezeFeePerMonth",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GracePeriodDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GrandfatherExistingPrice",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GuestPassResetPeriodDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestPassesIncluded",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasWaitlist",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IconName",
                table: "MembershipTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncludedFacilities",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncludesClasses",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludesGym",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplimentary",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDayPass",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromotional",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrial",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "JoiningFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LifetimeFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxBookingsPerDay",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxFreezeDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxMembers",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxSessionsPerMonth",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinCommitmentMonths",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinFamilyMembers",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinFreezeNoticeDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewPriceAfterIncrease",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextPriceIncreaseDate",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProRataEnabled",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProRataMinDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionEndDate",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionStartDate",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PromotionalPrice",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuarterlyFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RenewalReminderDays",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequireCancellationReason",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "MembershipTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOnWebsite",
                table: "MembershipTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TrialDurationDays",
                table: "MembershipTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MembershipTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpgradeToIds",
                table: "MembershipTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeeklyFee",
                table: "MembershipTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AccessSuspended",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "Memberships",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BillingCycle",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationEffectiveDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CancellationFeeCharged",
                table: "Memberships",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationFeedback",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancellationReason",
                table: "Memberships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationRequestDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledBy",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangeReason",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsecutivePaymentsOnTime",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Memberships",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Memberships",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "Memberships",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Memberships",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "Memberships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EligibleForReinstatement",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FreezeEndDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreezeNotes",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FreezeReason",
                table: "Memberships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FreezeStartDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FreezeYearResetDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GracePeriodEndDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GracePeriodNoticeSent",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuestPassResetDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestPassesUsed",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "InGracePeriod",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFrozen",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "JoiningFeePaid",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMissedPaymentDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRenewalDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MissedPaymentCount",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalStartDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousMembershipTypeId",
                table: "Memberships",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RenewalCount",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RenewalReminderSent",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RenewalReminderSentDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuspendedUntil",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuspensionReason",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpgradeDowngradeDate",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GuestPasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QRCodeData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuestLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuestEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GuestPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HostMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsFromMemberAllocation = table.Column<bool>(type: "bit", nullable: false),
                    ValidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidFromTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ValidUntilTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsMultiDayPass = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: true),
                    AllowedFacilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludesClasses = table.Column<bool>(type: "bit", nullable: false),
                    MaxSessions = table.Column<int>(type: "int", nullable: true),
                    SessionsUsed = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsComplimentary = table.Column<bool>(type: "bit", nullable: false),
                    ConvertedToMember = table.Column<bool>(type: "bit", nullable: false),
                    ConvertedMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConversionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaiverSigned = table.Column<bool>(type: "bit", nullable: false),
                    WaiverSignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaiverSignatureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestPasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestPasses_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestPasses_Members_ConvertedMemberId",
                        column: x => x.ConvertedMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuestPasses_Members_HostMemberId",
                        column: x => x.HostMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuestPasses_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PercentageOff = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FixedAmountOff = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxTotalUses = table.Column<int>(type: "int", nullable: true),
                    CurrentUseCount = table.Column<int>(type: "int", nullable: false),
                    MaxUsesPerMember = table.Column<int>(type: "int", nullable: true),
                    FirstTimeJoinersOnly = table.Column<bool>(type: "bit", nullable: false),
                    MinTenureMonths = table.Column<int>(type: "int", nullable: true),
                    MaxTenureMonths = table.Column<int>(type: "int", nullable: true),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    MinFamilySize = table.Column<int>(type: "int", nullable: true),
                    RequiredMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcludedMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresReferral = table.Column<bool>(type: "bit", nullable: false),
                    ReferrerMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanStackWithOther = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CorporatePartnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CorporatePartnerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CorporateMinMembers = table.Column<int>(type: "int", nullable: true),
                    SeasonalMonths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipDiscounts_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipDiscounts_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipFreezes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    ReasonDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportingDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedByStaff = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeDuringFreeze = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FeePaid = table.Column<bool>(type: "bit", nullable: false),
                    FreezePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OriginalMembershipEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendedMembershipEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipFreezes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipFreezes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipFreezes_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipWaitlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    JoinedWaitlistDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OfferSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferValidDays = table.Column<int>(type: "int", nullable: false),
                    OfferCount = table.Column<int>(type: "int", nullable: false),
                    LastOfferDeclinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeclineReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedMembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RemovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovalReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityScore = table.Column<int>(type: "int", nullable: false),
                    IsVIP = table.Column<bool>(type: "bit", nullable: false),
                    ReferredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReferringMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastNotificationSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipWaitlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipWaitlists_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipWaitlists_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipWaitlists_Members_ReferringMemberId",
                        column: x => x.ReferringMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipWaitlists_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipWaitlists_Memberships_CreatedMembershipId",
                        column: x => x.CreatedMembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PreviousMembershipTypeId",
                table: "Memberships",
                column: "PreviousMembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestPasses_ClubId_PassCode",
                table: "GuestPasses",
                columns: new[] { "ClubId", "PassCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestPasses_ConvertedMemberId",
                table: "GuestPasses",
                column: "ConvertedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestPasses_HostMemberId",
                table: "GuestPasses",
                column: "HostMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestPasses_PaymentId",
                table: "GuestPasses",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipDiscounts_ClubId_PromoCode",
                table: "MembershipDiscounts",
                columns: new[] { "ClubId", "PromoCode" },
                unique: true,
                filter: "[PromoCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipDiscounts_MembershipTypeId",
                table: "MembershipDiscounts",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipFreezes_ClubId_MembershipId",
                table: "MembershipFreezes",
                columns: new[] { "ClubId", "MembershipId" });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipFreezes_MemberId",
                table: "MembershipFreezes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipFreezes_MembershipId",
                table: "MembershipFreezes",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipWaitlists_ClubId_MembershipTypeId_Position",
                table: "MembershipWaitlists",
                columns: new[] { "ClubId", "MembershipTypeId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipWaitlists_CreatedMembershipId",
                table: "MembershipWaitlists",
                column: "CreatedMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipWaitlists_MemberId",
                table: "MembershipWaitlists",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipWaitlists_MembershipTypeId",
                table: "MembershipWaitlists",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipWaitlists_ReferringMemberId",
                table: "MembershipWaitlists",
                column: "ReferringMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipTypes_PreviousMembershipTypeId",
                table: "Memberships",
                column: "PreviousMembershipTypeId",
                principalTable: "MembershipTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipTypes_PreviousMembershipTypeId",
                table: "Memberships");

            migrationBuilder.DropTable(
                name: "GuestPasses");

            migrationBuilder.DropTable(
                name: "MembershipDiscounts");

            migrationBuilder.DropTable(
                name: "MembershipFreezes");

            migrationBuilder.DropTable(
                name: "MembershipWaitlists");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_PreviousMembershipTypeId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "AccessType",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AdditionalMemberFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AdminFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AdvanceBookingDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowAnnualPayment",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowDowngrade",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowFreeze",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowMonthlyPayment",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowQuarterlyPayment",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AllowUpgrade",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AutoRenewDefault",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AvailableUntil",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "BiannualFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "CancellationNoticeDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "CurrentMemberCount",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "DefaultBillingCycle",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "DowngradeToIds",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "EarlyCancellationFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ExcludedFacilities",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "FortnightlyFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "FreezeFeePerMonth",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "GracePeriodDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "GrandfatherExistingPrice",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "GuestPassResetPeriodDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "GuestPassesIncluded",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "HasWaitlist",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IconName",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IncludedFacilities",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IncludesClasses",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IncludesGym",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IsComplimentary",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IsDayPass",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IsPromotional",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "IsTrial",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "JoiningFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "LifetimeFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MaxBookingsPerDay",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MaxFreezeDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MaxMembers",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MaxSessionsPerMonth",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MinCommitmentMonths",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MinFamilyMembers",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "MinFreezeNoticeDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "NewPriceAfterIncrease",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "NextPriceIncreaseDate",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ProRataEnabled",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ProRataMinDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "PromotionEndDate",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "PromotionStartDate",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "PromotionalPrice",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "QuarterlyFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "RenewalReminderDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "RequireCancellationReason",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "ShowOnWebsite",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "TrialDurationDays",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "UpgradeToIds",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "WeeklyFee",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "AccessSuspended",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "BillingCycle",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationEffectiveDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationFeeCharged",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationFeedback",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancellationRequestDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "ChangeReason",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "ConsecutivePaymentsOnTime",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "EligibleForReinstatement",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "FreezeEndDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "FreezeNotes",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "FreezeReason",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "FreezeStartDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "FreezeYearResetDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "GracePeriodEndDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "GracePeriodNoticeSent",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "GuestPassResetDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "GuestPassesUsed",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "InGracePeriod",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "IsFrozen",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "JoiningFeePaid",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "LastMissedPaymentDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "LastRenewalDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MissedPaymentCount",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "OriginalStartDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "PreviousMembershipTypeId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "RenewalCount",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "RenewalReminderSent",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "RenewalReminderSentDate",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "SuspendedUntil",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "SuspensionReason",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "UpgradeDowngradeDate",
                table: "Memberships");

            migrationBuilder.RenameColumn(
                name: "TotalFreezeDaysUsed",
                table: "Memberships",
                newName: "PaymentType");
        }
    }
}
