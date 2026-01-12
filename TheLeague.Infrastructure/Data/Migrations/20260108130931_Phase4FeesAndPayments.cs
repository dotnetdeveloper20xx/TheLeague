using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase4FeesAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountLast4",
                table: "Payments",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankTransferReference",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardBrand",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardExpiryMonth",
                table: "Payments",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardExpiryYear",
                table: "Payments",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardLast4",
                table: "Payments",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalReference",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureCode",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeeId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeeType",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoCardlessPaymentId",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstallmentId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartialPayment",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartiallyRefunded",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefunded",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRetryDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentPaymentId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayPalOrderId",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentPlanId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProcessingFee",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptSent",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptSentDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecurringPaymentId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringSequence",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefundId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RefundedAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingBalance",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettlementDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeChargeId",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TipAmount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountOwed",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    FrequencyInterval = table.Column<int>(type: "int", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDayOfMonth = table.Column<int>(type: "int", nullable: true),
                    HasLateFee = table.Column<bool>(type: "bit", nullable: false),
                    LateFeeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LateFeePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    GracePeriodDays = table.Column<int>(type: "int", nullable: true),
                    LateFeeStartDay = table.Column<int>(type: "int", nullable: true),
                    AppliesToAll = table.Column<bool>(type: "bit", nullable: false),
                    ApplicableMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicableMemberCategories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    AllowEarlyPaymentDiscount = table.Column<bool>(type: "bit", nullable: false),
                    EarlyPaymentDiscountPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    EarlyPaymentDays = table.Column<int>(type: "int", nullable: true),
                    AllowPartialPayment = table.Column<bool>(type: "bit", nullable: false),
                    AllowPaymentPlan = table.Column<bool>(type: "bit", nullable: false),
                    MaxInstallments = table.Column<int>(type: "int", nullable: true),
                    IsRefundable = table.Column<bool>(type: "bit", nullable: false),
                    RefundableDays = table.Column<int>(type: "int", nullable: true),
                    AutoGenerate = table.Column<bool>(type: "bit", nullable: false),
                    AutoGenerateDaysBefore = table.Column<int>(type: "int", nullable: true),
                    AutoRemind = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSchedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    GLAccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fees_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoidedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    TaxExemptReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CollectionStatus = table.Column<int>(type: "int", nullable: false),
                    BillingName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BillingAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BillingCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BillingPostcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BillingCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BillingEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PaymentTermsDays = table.Column<int>(type: "int", nullable: false),
                    PaymentInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowPartialPayment = table.Column<bool>(type: "bit", nullable: false),
                    AllowOnlinePayment = table.Column<bool>(type: "bit", nullable: false),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: false),
                    DaysOverdue = table.Column<int>(type: "int", nullable: false),
                    LateFeeApplied = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LateFeeAppliedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemindersSent = table.Column<int>(type: "int", nullable: false),
                    LastReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalNoticeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicViewUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorporateInvoice = table.Column<bool>(type: "bit", nullable: false),
                    CorporateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrimaryMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoidReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoidedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Members_PrimaryMemberId",
                        column: x => x.PrimaryMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ServicePeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GLAccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItems_Fees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SetupFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    TotalInterest = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TotalInstallments = table.Column<int>(type: "int", nullable: false),
                    PaidInstallments = table.Column<int>(type: "int", nullable: false),
                    RemainingInstallments = table.Column<int>(type: "int", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FinalInstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    FrequencyInterval = table.Column<int>(type: "int", nullable: true),
                    PaymentDayOfMonth = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsAutoDebit = table.Column<bool>(type: "bit", nullable: false),
                    DefaultPaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MissedPayments = table.Column<int>(type: "int", nullable: false),
                    MaxMissedPayments = table.Column<int>(type: "int", nullable: false),
                    LateFeePerMissed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalLateFees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AgreementSigned = table.Column<bool>(type: "bit", nullable: false),
                    AgreementSignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgreementDocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyCancellationFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsPaused = table.Column<bool>(type: "bit", nullable: false),
                    PausedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResumeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PauseReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentPlans_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentPlans_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentPlans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentPlans_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CollectionStatus = table.Column<int>(type: "int", nullable: false),
                    LastStatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Current = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Overdue30 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Overdue60 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Overdue90 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Overdue120Plus = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OldestDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalOverdueItems = table.Column<int>(type: "int", nullable: false),
                    LastCreditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiringCredit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ExpiringCreditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPaidAllTime = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPaidThisYear = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalChargedAllTime = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalChargedThisYear = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalRefundedAllTime = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalWrittenOff = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ConsecutiveOnTimePayments = table.Column<int>(type: "int", nullable: false),
                    TotalLatePayments = table.Column<int>(type: "int", nullable: false),
                    TotalMissedPayments = table.Column<int>(type: "int", nullable: false),
                    AveragePaymentDelay = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PaymentRating = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HasAutoPaySetup = table.Column<bool>(type: "bit", nullable: false),
                    DefaultPaymentMethodId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DefaultPaymentMethodType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultPaymentMethodLast4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsAccessRestricted = table.Column<bool>(type: "bit", nullable: false),
                    AccessRestrictedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessRestrictionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasPaymentArrangement = table.Column<bool>(type: "bit", nullable: false),
                    PaymentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrangementNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastReminderSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemindersSentThisMonth = table.Column<int>(type: "int", nullable: false),
                    DoNotContact = table.Column<bool>(type: "bit", nullable: false),
                    DoNotContactUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberBalances_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberBalances_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberBalances_PaymentPlans_PaymentPlanId",
                        column: x => x.PaymentPlanId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentInstallments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallmentNumber = table.Column<int>(type: "int", nullable: false),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LateFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: false),
                    DaysOverdue = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAutoDebit = table.Column<bool>(type: "bit", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    LastRetryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverdueNoticeSent = table.Column<bool>(type: "bit", nullable: false),
                    OverdueNoticeSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInstallments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInstallments_PaymentPlans_PaymentPlanId",
                        column: x => x.PaymentPlanId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentInstallments_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstallmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReminderSequence = table.Column<int>(type: "int", nullable: false),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaysOverdue = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ToEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ToPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MessageBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOpened = table.Column<bool>(type: "bit", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasBounced = table.Column<bool>(type: "bit", nullable: false),
                    BounceReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEscalation = table.Column<bool>(type: "bit", nullable: false),
                    EscalationLevel = table.Column<int>(type: "int", nullable: false),
                    EscalationAction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EscalationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasResponse = table.Column<bool>(type: "bit", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentReceived = table.Column<bool>(type: "bit", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    LastRetryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_CommunicationTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "CommunicationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_PaymentInstallments_InstallmentId",
                        column: x => x.InstallmentId,
                        principalTable: "PaymentInstallments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_PaymentPlans_PaymentPlanId",
                        column: x => x.PaymentPlanId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReminders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BalanceTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FeeType = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_MemberBalances_MemberBalanceId",
                        column: x => x.MemberBalanceId,
                        principalTable: "MemberBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefundNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginalPaymentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsFullRefund = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    ReasonDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefundMethod = table.Column<int>(type: "int", nullable: false),
                    RefundToOriginalMethod = table.Column<bool>(type: "bit", nullable: false),
                    RefundToAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StripeRefundId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayPalRefundId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankTransferReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefundAsCredit = table.Column<bool>(type: "bit", nullable: false),
                    BalanceTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportingDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedMembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AffectedEventTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MembershipCancelled = table.Column<bool>(type: "bit", nullable: false),
                    EventTicketCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refunds_BalanceTransactions_BalanceTransactionId",
                        column: x => x.BalanceTransactionId,
                        principalTable: "BalanceTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refunds_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FeeId",
                table: "Payments",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InstallmentId",
                table: "Payments",
                column: "InstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentPlanId",
                table: "Payments",
                column: "PaymentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_ClubId_MemberId_TransactionDate",
                table: "BalanceTransactions",
                columns: new[] { "ClubId", "MemberId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_InvoiceId",
                table: "BalanceTransactions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_MemberBalanceId",
                table: "BalanceTransactions",
                column: "MemberBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_MemberId",
                table: "BalanceTransactions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_PaymentId",
                table: "BalanceTransactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_RefundId",
                table: "BalanceTransactions",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_ClubId_Code",
                table: "Fees",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItems_FeeId",
                table: "InvoiceLineItems",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItems_InvoiceId",
                table: "InvoiceLineItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClubId_InvoiceNumber",
                table: "Invoices",
                columns: new[] { "ClubId", "InvoiceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_MemberId",
                table: "Invoices",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PrimaryMemberId",
                table: "Invoices",
                column: "PrimaryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberBalances_ClubId_MemberId",
                table: "MemberBalances",
                columns: new[] { "ClubId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberBalances_MemberId",
                table: "MemberBalances",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberBalances_PaymentPlanId",
                table: "MemberBalances",
                column: "PaymentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstallments_MemberId",
                table: "PaymentInstallments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstallments_PaymentId",
                table: "PaymentInstallments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstallments_PaymentPlanId_InstallmentNumber",
                table: "PaymentInstallments",
                columns: new[] { "PaymentPlanId", "InstallmentNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlans_ClubId_PlanReference",
                table: "PaymentPlans",
                columns: new[] { "ClubId", "PlanReference" },
                unique: true,
                filter: "[PlanReference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlans_InvoiceId",
                table: "PaymentPlans",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlans_MemberId",
                table: "PaymentPlans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlans_MembershipId",
                table: "PaymentPlans",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_ClubId_MemberId_ScheduledDate",
                table: "PaymentReminders",
                columns: new[] { "ClubId", "MemberId", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_InstallmentId",
                table: "PaymentReminders",
                column: "InstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_InvoiceId",
                table: "PaymentReminders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_MemberId",
                table: "PaymentReminders",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_MembershipId",
                table: "PaymentReminders",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_PaymentId",
                table: "PaymentReminders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_PaymentPlanId",
                table: "PaymentReminders",
                column: "PaymentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReminders_TemplateId",
                table: "PaymentReminders",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_BalanceTransactionId",
                table: "Refunds",
                column: "BalanceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ClubId_RefundNumber",
                table: "Refunds",
                columns: new[] { "ClubId", "RefundNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_MemberId",
                table: "Refunds",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PaymentId",
                table: "Refunds",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Fees_FeeId",
                table: "Payments",
                column: "FeeId",
                principalTable: "Fees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentInstallments_InstallmentId",
                table: "Payments",
                column: "InstallmentId",
                principalTable: "PaymentInstallments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentPlans_PaymentPlanId",
                table: "Payments",
                column: "PaymentPlanId",
                principalTable: "PaymentPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceTransactions_Refunds_RefundId",
                table: "BalanceTransactions",
                column: "RefundId",
                principalTable: "Refunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Fees_FeeId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentInstallments_InstallmentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentPlans_PaymentPlanId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceTransactions_Invoices_InvoiceId",
                table: "BalanceTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentPlans_Invoices_InvoiceId",
                table: "PaymentPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceTransactions_MemberBalances_MemberBalanceId",
                table: "BalanceTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceTransactions_Refunds_RefundId",
                table: "BalanceTransactions");

            migrationBuilder.DropTable(
                name: "InvoiceLineItems");

            migrationBuilder.DropTable(
                name: "PaymentReminders");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "PaymentInstallments");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MemberBalances");

            migrationBuilder.DropTable(
                name: "PaymentPlans");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "BalanceTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_FeeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_InstallmentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentPlanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BankAccountLast4",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BankTransferReference",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardBrand",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardExpiryMonth",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardExpiryYear",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardLast4",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExternalReference",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FailureCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FeeType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GoCardlessPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InstallmentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsPartialPayment",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsPartiallyRefunded",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsRefunded",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "LastRetryDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OriginalAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ParentPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayPalOrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentPlanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProcessingFee",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceiptSent",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceiptSentDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RecurringPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RecurringSequence",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundedAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SettlementDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeChargeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TipAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TotalAmountOwed",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Payments");
        }
    }
}
