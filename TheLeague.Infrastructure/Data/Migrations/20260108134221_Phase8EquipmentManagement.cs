using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase8EquipmentManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BarCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    ConditionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnLoanQuantity = table.Column<int>(type: "int", nullable: false),
                    MinimumStock = table.Column<int>(type: "int", nullable: false),
                    LowStockAlert = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    WeightUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Dimensions = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Shelf = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PurchaseCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ReplacementCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepreciationRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    SalvageValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UsefulLifeYears = table.Column<int>(type: "int", nullable: true),
                    HasWarranty = table.Column<bool>(type: "bit", nullable: false),
                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarrantyProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WarrantyTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarrantyContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInsured = table.Column<bool>(type: "bit", nullable: false),
                    InsurancePolicy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InsuranceExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsuredValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsLoanable = table.Column<bool>(type: "bit", nullable: false),
                    RequiresDeposit = table.Column<bool>(type: "bit", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    MaxLoanDays = table.Column<int>(type: "int", nullable: true),
                    DailyLoanFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LateFeePerDay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LoanTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembersOnly = table.Column<bool>(type: "bit", nullable: false),
                    AllowedMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    RequiresCertification = table.Column<bool>(type: "bit", nullable: false),
                    RequiredCertifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaintenanceIntervalDays = table.Column<int>(type: "int", nullable: true),
                    MaintenanceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastInspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextInspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectionFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassedLastInspection = table.Column<bool>(type: "bit", nullable: false),
                    InspectionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GalleryUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManualUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLoanCount = table.Column<int>(type: "int", nullable: false),
                    TotalUsageHours = table.Column<int>(type: "int", nullable: false),
                    LastUsedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalLoanRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsRetired = table.Column<bool>(type: "bit", nullable: false),
                    RetiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetiredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RetirementReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisposalMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisposalValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SafetyNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipment_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentLoans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaysOverdue = table.Column<int>(type: "int", nullable: true),
                    ConditionAtLoan = table.Column<int>(type: "int", nullable: false),
                    ConditionAtReturn = table.Column<int>(type: "int", nullable: true),
                    ConditionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositRequired = table.Column<bool>(type: "bit", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositPaid = table.Column<bool>(type: "bit", nullable: false),
                    DepositPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepositPaymentRef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepositReturned = table.Column<bool>(type: "bit", nullable: false),
                    DepositReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepositDeduction = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DeductionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LateFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DamageFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollectedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CollectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReturnLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentLoans_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedDuration = table.Column<int>(type: "int", nullable: true),
                    ActualDuration = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ConditionBefore = table.Column<int>(type: "int", nullable: true),
                    ConditionAfter = table.Column<int>(type: "int", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PartsUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartsCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LaborCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrenceIntervalDays = table.Column<int>(type: "int", nullable: true),
                    NextScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPerformed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuesFound = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenances_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenances_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenances_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvertedToLoan = table.Column<bool>(type: "bit", nullable: false),
                    LoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConvertedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_EquipmentLoans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "EquipmentLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReservations_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ClubId_Code",
                table: "Equipment",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_FacilityId",
                table: "Equipment",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_PurchaseOrderId",
                table: "Equipment",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_VendorId",
                table: "Equipment",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_VenueId",
                table: "Equipment",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_ClubId_LoanNumber",
                table: "EquipmentLoans",
                columns: new[] { "ClubId", "LoanNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_EquipmentId",
                table: "EquipmentLoans",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_EventId",
                table: "EquipmentLoans",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_MemberId",
                table: "EquipmentLoans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_PaymentId",
                table: "EquipmentLoans",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLoans_SessionId",
                table: "EquipmentLoans",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenances_EquipmentId",
                table: "EquipmentMaintenances",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenances_ExpenseId",
                table: "EquipmentMaintenances",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenances_VendorId",
                table: "EquipmentMaintenances",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_ClubId_ReservationNumber",
                table: "EquipmentReservations",
                columns: new[] { "ClubId", "ReservationNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_EquipmentId",
                table: "EquipmentReservations",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_EventId",
                table: "EquipmentReservations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_LoanId",
                table: "EquipmentReservations",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_MemberId",
                table: "EquipmentReservations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservations_SessionId",
                table: "EquipmentReservations",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMaintenances");

            migrationBuilder.DropTable(
                name: "EquipmentReservations");

            migrationBuilder.DropTable(
                name: "EquipmentLoans");

            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
