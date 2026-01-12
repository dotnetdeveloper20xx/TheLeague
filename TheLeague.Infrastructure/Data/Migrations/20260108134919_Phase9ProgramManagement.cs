using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLeague.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase9ProgramManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    AgeGroup = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    TargetGender = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationWeeks = table.Column<int>(type: "int", nullable: true),
                    TotalSessions = table.Column<int>(type: "int", nullable: true),
                    SessionDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationOpenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyBirdDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiresApplication = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinParticipants = table.Column<int>(type: "int", nullable: true),
                    MaxParticipants = table.Column<int>(type: "int", nullable: true),
                    CurrentEnrollment = table.Column<int>(type: "int", nullable: false),
                    WaitlistCount = table.Column<int>(type: "int", nullable: false),
                    AllowWaitlist = table.Column<bool>(type: "bit", nullable: false),
                    WaitlistLimit = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EarlyBirdPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MemberPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NonMemberPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositRequired = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AllowInstallments = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfInstallments = table.Column<int>(type: "int", nullable: true),
                    FeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SiblingDiscount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MultiProgramDiscount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryInstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstructorNames = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StudentToInstructorRatio = table.Column<int>(type: "int", nullable: true),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrerequisiteProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequiredSkillLevel = table.Column<int>(type: "int", nullable: true),
                    RequiredCertifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OffersQualification = table.Column<bool>(type: "bit", nullable: false),
                    QualificationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    QualificationBody = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CertificateTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentProvided = table.Column<bool>(type: "bit", nullable: false),
                    EquipmentIncluded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembersOnly = table.Column<bool>(type: "bit", nullable: false),
                    AllowedMembershipTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CancellationNoticeDays = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GalleryUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalEnrollmentsAllTime = table.Column<int>(type: "int", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShowOnWebsite = table.Column<bool>(type: "bit", nullable: false),
                    AllowOnlineRegistration = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructorNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Fees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Members_PrimaryInstructorId",
                        column: x => x.PrimaryInstructorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Programs_PrerequisiteProgramId",
                        column: x => x.PrerequisiteProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnrollmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaitlistPosition = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionsAttended = table.Column<int>(type: "int", nullable: false),
                    SessionsMissed = table.Column<int>(type: "int", nullable: false),
                    AttendanceRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    SkillLevelAtStart = table.Column<int>(type: "int", nullable: true),
                    SkillLevelAtEnd = table.Column<int>(type: "int", nullable: true),
                    AssessmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    FinalScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    GradeAwarded = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    DepositPaid = table.Column<bool>(type: "bit", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WithdrawnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WithdrawnBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WithdrawalReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundProcessed = table.Column<bool>(type: "bit", nullable: false),
                    IsTransferred = table.Column<bool>(type: "bit", nullable: false),
                    TransferredFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransferredToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CertificateAwarded = table.Column<bool>(type: "bit", nullable: false),
                    CertificateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactRelation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MedicalFormSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ConsentFormSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    MedicalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_FamilyMembers_FamilyMemberId",
                        column: x => x.FamilyMemberId,
                        principalTable: "FamilyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_ProgramEnrollments_TransferredFromId",
                        column: x => x.TransferredFromId,
                        principalTable: "ProgramEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramInstructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Certifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SessionRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramInstructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramInstructors_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramInstructors_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RescheduledFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsMakeUp = table.Column<bool>(type: "bit", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstructorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    AttendanceCount = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructorNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearningObjectives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialsCovered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceUrls = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramSessions_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramSessions_Members_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramSessions_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramSessions_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AwardingBody = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AwardedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AwardedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NeverExpires = table.Column<bool>(type: "bit", nullable: false),
                    ValidityYears = table.Column<int>(type: "int", nullable: true),
                    RequiresRenewal = table.Column<bool>(type: "bit", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RenewalFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RenewalReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    VerificationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BadgeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberCertificates_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberCertificates_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberCertificates_ProgramEnrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "ProgramEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberCertificates_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramAttendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedInBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MinutesLate = table.Column<int>(type: "int", nullable: true),
                    MinutesEarly = table.Column<int>(type: "int", nullable: true),
                    ExcuseReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExcuseApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsMakeUp = table.Column<bool>(type: "bit", nullable: false),
                    MakeUpForSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MakeUpScheduledId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PerformanceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    InstructorFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramAttendances_ProgramEnrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "ProgramEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramAttendances_ProgramSessions_MakeUpForSessionId",
                        column: x => x.MakeUpForSessionId,
                        principalTable: "ProgramSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramAttendances_ProgramSessions_ProgramSessionId",
                        column: x => x.ProgramSessionId,
                        principalTable: "ProgramSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberCertificates_ClubId_CertificateNumber",
                table: "MemberCertificates",
                columns: new[] { "ClubId", "CertificateNumber" },
                unique: true,
                filter: "[CertificateNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCertificates_EnrollmentId",
                table: "MemberCertificates",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCertificates_MemberId",
                table: "MemberCertificates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCertificates_ProgramId",
                table: "MemberCertificates",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramAttendances_EnrollmentId",
                table: "ProgramAttendances",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramAttendances_MakeUpForSessionId",
                table: "ProgramAttendances",
                column: "MakeUpForSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramAttendances_ProgramSessionId_EnrollmentId",
                table: "ProgramAttendances",
                columns: new[] { "ProgramSessionId", "EnrollmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_ClubId_EnrollmentNumber",
                table: "ProgramEnrollments",
                columns: new[] { "ClubId", "EnrollmentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_FamilyMemberId",
                table: "ProgramEnrollments",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_InvoiceId",
                table: "ProgramEnrollments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_MemberId",
                table: "ProgramEnrollments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_PaymentId",
                table: "ProgramEnrollments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_ProgramId",
                table: "ProgramEnrollments",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_TransferredFromId",
                table: "ProgramEnrollments",
                column: "TransferredFromId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramInstructors_MemberId",
                table: "ProgramInstructors",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramInstructors_ProgramId_MemberId",
                table: "ProgramInstructors",
                columns: new[] { "ProgramId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ClubId_Code",
                table: "Programs",
                columns: new[] { "ClubId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_FacilityId",
                table: "Programs",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_FeeId",
                table: "Programs",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_PrerequisiteProgramId",
                table: "Programs",
                column: "PrerequisiteProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_PrimaryInstructorId",
                table: "Programs",
                column: "PrimaryInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_VenueId",
                table: "Programs",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSessions_FacilityId",
                table: "ProgramSessions",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSessions_InstructorId",
                table: "ProgramSessions",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSessions_ProgramId",
                table: "ProgramSessions",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSessions_VenueId",
                table: "ProgramSessions",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberCertificates");

            migrationBuilder.DropTable(
                name: "ProgramAttendances");

            migrationBuilder.DropTable(
                name: "ProgramInstructors");

            migrationBuilder.DropTable(
                name: "ProgramEnrollments");

            migrationBuilder.DropTable(
                name: "ProgramSessions");

            migrationBuilder.DropTable(
                name: "Programs");
        }
    }
}
