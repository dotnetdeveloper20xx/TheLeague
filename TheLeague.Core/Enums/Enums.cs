namespace TheLeague.Core.Enums;

public enum ClubType
{
    Cricket,
    Football,
    Rugby,
    Tennis,
    Golf,
    Hockey,
    Swimming,
    Athletics,
    MultiSport,
    CommunityGroup,
    YouthOrganization,
    Other
}

public enum PaymentProvider
{
    Stripe,
    PayPal
}

public enum MemberStatus
{
    Pending,
    Active,
    Expired,
    Suspended,
    Cancelled
}

public enum FamilyMemberRelation
{
    Spouse,
    Child,
    Sibling,
    Parent,
    Other
}

public enum MembershipPaymentType
{
    Annual,
    Monthly,
    PayAsYouGo
}

public enum MembershipStatus
{
    Active,
    PendingPayment,
    Expired,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    Cancelled
}

public enum PaymentMethod
{
    Stripe,
    PayPal,
    BankTransfer,
    Cash,
    Cheque,
    Other
}

public enum PaymentType
{
    Membership,
    EventTicket,
    SessionFee,
    Other
}

public enum SessionCategory
{
    AllAges,
    Juniors,
    Seniors,
    U7,
    U9,
    U11,
    U13,
    U15,
    U17,
    U19,
    Ladies,
    Mens,
    Mixed,
    Beginners,
    Advanced,
    Social,
    Competition
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    NoShow,
    Attended
}

public enum EventType
{
    Social,
    Tournament,
    AGM,
    Training,
    Fundraiser,
    Competition,
    Meeting,
    Presentation,
    Other
}

public enum RSVPResponse
{
    Attending,
    NotAttending,
    Maybe
}

public enum DocumentType
{
    ProfilePhoto,
    MedicalForm,
    ConsentForm,
    DBSCertificate,
    CoachingQualification,
    Other
}

public enum EmailType
{
    Welcome,
    PasswordReset,
    PaymentReminder,
    PaymentReceipt,
    BookingConfirmation,
    BookingCancellation,
    EventReminder,
    MembershipRenewal,
    BulkCommunication,
    WaitlistNotification
}

public enum EmailStatus
{
    Pending,
    Sent,
    Failed,
    Bounced
}

public enum CampaignStatus
{
    Draft,
    Scheduled,
    Sending,
    Completed,
    Failed
}

public enum UserRole
{
    SuperAdmin,
    ClubManager,
    Member,
    Coach,
    Staff
}

public enum Gender
{
    Male,
    Female,
    Other,
    PreferNotToSay
}

public enum ApplicationStatus
{
    Draft,
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    RequiresMoreInfo
}

public enum MemberNoteType
{
    General,
    Medical,
    Payment,
    Behavior,
    Achievement,
    Communication,
    Internal
}

public enum CustomFieldType
{
    Text,
    Number,
    Date,
    Boolean,
    Select,
    MultiSelect,
    TextArea
}

// Phase 3: Membership Plans & Pricing

public enum MembershipCategory
{
    Individual,
    Family,
    Corporate,
    Student,
    Senior,
    Junior,
    Couple,
    Lifetime,
    Honorary,
    Trial,
    DayPass
}

public enum BillingCycle
{
    Weekly,
    Fortnightly,
    Monthly,
    Quarterly,
    Biannual,
    Annual,
    Lifetime,
    OneTime
}

public enum AccessType
{
    FullAccess,       // All facilities, all times
    Limited,          // Specific facilities only
    PeakOnly,         // Peak hours only
    OffPeakOnly,      // Off-peak hours only
    WeekendOnly,      // Weekends only
    WeekdayOnly       // Weekdays only
}

public enum DiscountType
{
    EarlyBird,        // Early registration discount
    Loyalty,          // Tenure-based discount
    Family,           // Family/group discount
    Corporate,        // Corporate rate
    Seasonal,         // Seasonal promotion
    Referral,         // Referral discount
    PromoCode,        // Promotional code
    BundleDiscount,   // Multiple memberships discount
    Senior,           // Senior citizen discount
    Student           // Student discount
}

public enum FreezeReason
{
    Medical,
    Travel,
    Financial,
    Personal,
    Seasonal,
    Other
}

public enum CancellationReason
{
    Moving,
    Financial,
    Dissatisfied,
    NoLongerInterested,
    Medical,
    TimeConstraints,
    JoiningOtherClub,
    Other
}

public enum WaitlistStatus
{
    Waiting,
    Offered,
    Accepted,
    Declined,
    Expired,
    Removed
}

// Phase 4: Fees & Payments

public enum FeeType
{
    Membership,
    Registration,
    JoiningFee,
    AnnualSubscription,
    Activity,
    ClassFee,
    EventParticipation,
    FacilityBooking,
    EquipmentRental,
    LockerRental,
    Coaching,
    Training,
    LatePaymentPenalty,
    Administration,
    Insurance,
    GuestPass,
    Merchandise,
    Other
}

public enum FeeFrequency
{
    OneTime,
    PerSession,
    Daily,
    Weekly,
    Monthly,
    Quarterly,
    Biannual,
    Annual
}

public enum PaymentPlanStatus
{
    Active,
    Completed,
    Defaulted,
    Cancelled,
    Paused
}

public enum InstallmentStatus
{
    Pending,
    Scheduled,
    Paid,
    PartiallyPaid,
    Overdue,
    Failed,
    Waived,
    Cancelled
}

public enum RefundStatus
{
    Requested,
    Approved,
    Processing,
    Completed,
    Rejected,
    PartiallyRefunded,
    Failed
}

public enum RefundReason
{
    Cancellation,
    Duplicate,
    ServiceNotProvided,
    Overcharge,
    MembershipDowngrade,
    EventCancelled,
    CustomerRequest,
    ErrorCorrection,
    Other
}

public enum CollectionStatus
{
    Current,
    Overdue30,
    Overdue60,
    Overdue90,
    Overdue120Plus,
    InArrangement,
    SentToCollection,
    WrittenOff,
    Disputed
}

public enum InvoiceStatus
{
    Draft,
    Sent,
    Viewed,
    PartiallyPaid,
    Paid,
    Overdue,
    Voided,
    Disputed,
    WrittenOff
}

public enum ReminderType
{
    PaymentDue,
    PaymentOverdue,
    PaymentFailed,
    PaymentMethodExpiring,
    MembershipRenewal,
    InstallmentDue,
    FinalNotice
}

public enum TransactionType
{
    Payment,
    Refund,
    Credit,
    Debit,
    Adjustment,
    WriteOff,
    Transfer
}

// Phase 5: Financial Management

public enum AccountCategory
{
    Asset,
    Liability,
    Equity,
    Revenue,
    Expense
}

public enum AccountType
{
    // Asset accounts
    Cash,
    BankAccount,
    AccountsReceivable,
    PrepaidExpenses,
    Inventory,
    FixedAssets,

    // Liability accounts
    AccountsPayable,
    AccruedExpenses,
    DeferredRevenue,
    LongTermDebt,

    // Equity accounts
    RetainedEarnings,
    OwnersEquity,
    MemberCapital,

    // Revenue accounts
    MembershipIncome,
    EventIncome,
    FacilityIncome,
    MerchandiseIncome,
    DonationsIncome,
    GrantsIncome,
    OtherIncome,

    // Expense accounts
    Salaries,
    Utilities,
    Rent,
    Insurance,
    Marketing,
    Equipment,
    Supplies,
    Maintenance,
    ProfessionalFees,
    BankFees,
    OtherExpenses
}

public enum JournalEntryStatus
{
    Draft,
    Pending,
    Posted,
    Voided,
    Reversed
}

public enum ReconciliationStatus
{
    NotStarted,
    InProgress,
    PendingReview,
    Completed,
    Discrepancy,
    Approved
}

public enum TaxRateType
{
    StandardRate,
    ReducedRate,
    ZeroRated,
    Exempt,
    OutOfScope
}

public enum FinancialReportType
{
    IncomeStatement,
    BalanceSheet,
    CashFlow,
    RevenueByCategory,
    ExpenseByCategory,
    MembershipRevenue,
    EventRevenue,
    AgingReport,
    TaxSummary,
    BudgetVsActual,
    MonthlySummary,
    AnnualSummary,
    CustomReport
}

public enum BudgetStatus
{
    Draft,
    PendingApproval,
    Approved,
    Active,
    Frozen,
    Closed,
    Archived
}

public enum FiscalPeriodStatus
{
    Future,
    Current,
    Closed,
    Locked,
    Archived
}

public enum AuditActionType
{
    Create,
    Update,
    Delete,
    Approve,
    Reject,
    Post,
    Void,
    Reverse,
    Reconcile,
    Export,
    Import
}

// Phase 6: Expense Management

public enum ExpenseCategory
{
    Salaries,
    Wages,
    Utilities,
    Rent,
    Insurance,
    Maintenance,
    Equipment,
    Supplies,
    Marketing,
    Travel,
    Catering,
    Printing,
    Postage,
    Telecommunications,
    Software,
    Legal,
    Accounting,
    Banking,
    Taxes,
    Training,
    Subscriptions,
    Licenses,
    Security,
    Cleaning,
    Awards,
    Prizes,
    Entertainment,
    Miscellaneous,
    Other
}

public enum ExpenseStatus
{
    Draft,
    Submitted,
    PendingApproval,
    Approved,
    Rejected,
    Processing,
    Paid,
    Cancelled,
    Voided
}

public enum VendorStatus
{
    Pending,
    Active,
    Inactive,
    Suspended,
    Blocked
}

public enum VendorType
{
    Supplier,
    Contractor,
    ServiceProvider,
    Utility,
    Government,
    Individual,
    Other
}

public enum PurchaseOrderStatus
{
    Draft,
    PendingApproval,
    Approved,
    Sent,
    PartiallyReceived,
    Received,
    Closed,
    Cancelled,
    Voided
}

public enum ApprovalStatus
{
    Pending,
    Approved,
    Rejected,
    Escalated,
    Withdrawn
}

public enum PaymentTerms
{
    DueOnReceipt,
    Net7,
    Net14,
    Net30,
    Net45,
    Net60,
    Net90,
    EndOfMonth,
    Custom
}

// Phase 7: Facility Management

public enum FacilityType
{
    Court,          // Tennis, basketball, squash courts
    Pool,           // Swimming pools
    Field,          // Football, cricket, rugby fields
    Track,          // Running tracks
    Gym,            // Fitness areas
    Studio,         // Dance, yoga, fitness studios
    MeetingRoom,    // Conference/meeting rooms
    ChangingRoom,   // Locker rooms
    ClubHouse,      // Main building
    Parking,        // Parking areas
    Equipment,      // Equipment storage
    Bar,            // Bar/refreshment areas
    Kitchen,        // Kitchen facilities
    Office,         // Administrative offices
    Other
}

public enum FacilityStatus
{
    Available,
    Occupied,
    Reserved,
    Maintenance,
    Closed,
    OutOfService
}

public enum BookingSlotStatus
{
    Available,
    Booked,
    Blocked,
    Maintenance,
    Holiday,
    Cancelled
}

public enum FacilityBookingStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    Completed,
    Cancelled,
    NoShow
}

public enum MaintenanceType
{
    Routine,        // Regular scheduled maintenance
    Preventive,     // Preventive maintenance
    Corrective,     // Fix issues/breakdowns
    Emergency,      // Emergency repairs
    Upgrade,        // Improvements/upgrades
    Inspection,     // Safety inspections
    Cleaning,       // Deep cleaning
    Seasonal        // Seasonal preparation
}

public enum MaintenanceStatus
{
    Scheduled,
    InProgress,
    OnHold,
    Completed,
    Cancelled,
    Deferred
}

public enum MaintenancePriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum DayOfWeekFlag
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64,
    Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
    Weekend = Saturday | Sunday,
    AllDays = Weekdays | Weekend
}

public enum PricingType
{
    Flat,           // Fixed price
    PerHour,        // Hourly rate
    PerHalfHour,    // Half-hourly rate
    PerSession,     // Session-based
    PerPerson,      // Per person
    Peak,           // Peak time pricing
    OffPeak,        // Off-peak pricing
    Member,         // Member rate
    NonMember       // Non-member rate
}

// Phase 8: Equipment Management

public enum EquipmentCategory
{
    Sports,         // Sports equipment (balls, rackets, etc.)
    Training,       // Training aids
    Safety,         // Safety equipment (helmets, pads)
    Electronics,    // Electronics (timing systems, scoreboards)
    Furniture,      // Furniture (chairs, tables)
    Tools,          // Maintenance tools
    Cleaning,       // Cleaning equipment
    Medical,        // First aid, medical equipment
    Office,         // Office equipment
    Kitchen,        // Kitchen equipment
    Audio,          // Audio/visual equipment
    Signage,        // Signs, banners
    Transport,      // Trolleys, carts
    Other
}

public enum EquipmentCondition
{
    New,
    Excellent,
    Good,
    Fair,
    Poor,
    NeedsRepair,
    Damaged,
    Decommissioned
}

public enum EquipmentStatus
{
    Available,
    InUse,
    Reserved,
    OnLoan,
    Maintenance,
    Missing,
    Retired,
    Disposed
}

public enum LoanStatus
{
    Requested,
    Approved,
    Active,
    Overdue,
    Returned,
    ReturnedDamaged,
    Lost,
    Cancelled
}

public enum EquipmentMaintenanceType
{
    Routine,
    Repair,
    Cleaning,
    Inspection,
    Calibration,
    Replacement,
    Upgrade
}

// Phase 9: Programs & Activities

public enum ProgramType
{
    Course,         // Structured learning (swimming lessons, coaching)
    Camp,           // Multi-day intensive programs
    Class,          // Recurring classes (fitness, yoga)
    Workshop,       // One-off skill building sessions
    Clinic,         // Short instructional sessions
    League,         // Competitive leagues
    Academy,        // Long-term development programs
    Squad,          // Team training groups
    PrivateLesson,  // One-on-one instruction
    GroupLesson,    // Small group instruction
    Trial,          // Trial/taster sessions
    Holiday,        // Holiday programs
    Other
}

public enum ProgramStatus
{
    Draft,
    Published,
    OpenForRegistration,
    RegistrationClosed,
    InProgress,
    Completed,
    Cancelled,
    Archived
}

public enum EnrollmentStatus
{
    Pending,
    Confirmed,
    WaitListed,
    Attended,
    Withdrawn,
    Transferred,
    Completed,
    Failed,
    Cancelled
}

public enum SkillLevel
{
    Beginner,
    Elementary,
    Intermediate,
    UpperIntermediate,
    Advanced,
    Expert,
    AllLevels
}

public enum AgeGroup
{
    Infant,         // 0-2
    Toddler,        // 3-5
    Child,          // 6-8
    PreTeen,        // 9-12
    Teen,           // 13-17
    Adult,          // 18-64
    Senior,         // 65+
    AllAges
}

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    Excused,
    NoShow,
    MakeUp,
    Pending
}

public enum CertificateStatus
{
    NotStarted,
    InProgress,
    Pending,
    Awarded,
    Expired,
    Revoked
}

// Phase 10: Events & Competitions Enhanced

public enum EventStatus
{
    Draft,
    Published,
    RegistrationOpen,
    RegistrationClosed,
    Upcoming,
    InProgress,
    Completed,
    Cancelled,
    Postponed,
    Archived
}

public enum EventRegistrationStatus
{
    Pending,
    Confirmed,
    WaitListed,
    CheckedIn,
    Attended,
    NoShow,
    Cancelled,
    Refunded
}

public enum TicketStatus
{
    Available,
    Reserved,
    Sold,
    CheckedIn,
    Used,
    Cancelled,
    Refunded,
    Expired
}

public enum CompetitionType
{
    League,             // Season-long league with standings
    Tournament,         // Knockout/bracket tournament
    Cup,                // Cup competition
    Friendly,           // Friendly matches
    Championship,       // Championship event
    Qualifier,          // Qualifying rounds
    Playoff,            // Playoff rounds
    RoundRobin,         // Round-robin format
    Ladder,             // Ladder competition
    TimeTrial,          // Timed events
    Other
}

public enum CompetitionStatus
{
    Draft,
    Published,
    RegistrationOpen,
    RegistrationClosed,
    DrawComplete,
    InProgress,
    Completed,
    Cancelled,
    Postponed,
    Archived
}

public enum MatchStatus
{
    Scheduled,
    Confirmed,
    InProgress,
    Completed,
    Postponed,
    Cancelled,
    Walkover,
    Bye,
    Abandoned,
    Disputed
}

public enum MatchResult
{
    NotPlayed,
    HomeWin,
    AwayWin,
    Draw,
    HomeWalkover,
    AwayWalkover,
    BothWalkover,
    Void
}

public enum TeamStatus
{
    Registered,
    Confirmed,
    Withdrawn,
    Disqualified,
    Eliminated,
    Active,
    Champion,
    RunnerUp
}

public enum ParticipantRole
{
    Player,
    Captain,
    ViceCaptain,
    Coach,
    Manager,
    Official,
    Substitute,
    Reserve
}
