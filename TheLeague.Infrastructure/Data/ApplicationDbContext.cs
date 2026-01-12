using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheLeague.Core.Entities;

namespace TheLeague.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantService? _tenantService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<ClubSettings> ClubSettings => Set<ClubSettings>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
    public DbSet<MembershipType> MembershipTypes => Set<MembershipType>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<RecurringSchedule> RecurringSchedules => Set<RecurringSchedule>();
    public DbSet<SessionBooking> SessionBookings => Set<SessionBooking>();
    public DbSet<RecurringBooking> RecurringBookings => Set<RecurringBooking>();
    public DbSet<Waitlist> Waitlists => Set<Waitlist>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventTicket> EventTickets => Set<EventTicket>();
    public DbSet<EventRSVP> EventRSVPs => Set<EventRSVP>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<MemberDocument> MemberDocuments => Set<MemberDocument>();
    public DbSet<MemberNote> MemberNotes => Set<MemberNote>();
    public DbSet<CustomFieldDefinition> CustomFieldDefinitions => Set<CustomFieldDefinition>();
    public DbSet<CommunicationTemplate> CommunicationTemplates => Set<CommunicationTemplate>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
    public DbSet<BulkEmailCampaign> BulkEmailCampaigns => Set<BulkEmailCampaign>();
    public DbSet<ClubAnalyticsSnapshot> ClubAnalyticsSnapshots => Set<ClubAnalyticsSnapshot>();

    // Phase 3: Membership Plans & Pricing
    public DbSet<MembershipDiscount> MembershipDiscounts => Set<MembershipDiscount>();
    public DbSet<MembershipFreeze> MembershipFreezes => Set<MembershipFreeze>();
    public DbSet<GuestPass> GuestPasses => Set<GuestPass>();
    public DbSet<MembershipWaitlist> MembershipWaitlists => Set<MembershipWaitlist>();

    // Phase 4: Fees & Payments
    public DbSet<Fee> Fees => Set<Fee>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<PaymentPlan> PaymentPlans => Set<PaymentPlan>();
    public DbSet<PaymentInstallment> PaymentInstallments => Set<PaymentInstallment>();
    public DbSet<MemberBalance> MemberBalances => Set<MemberBalance>();
    public DbSet<BalanceTransaction> BalanceTransactions => Set<BalanceTransaction>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<PaymentReminder> PaymentReminders => Set<PaymentReminder>();

    // Phase 5: Financial Management
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
    public DbSet<FiscalYear> FiscalYears => Set<FiscalYear>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<BankReconciliation> BankReconciliations => Set<BankReconciliation>();
    public DbSet<BankReconciliationLine> BankReconciliationLines => Set<BankReconciliationLine>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetLine> BudgetLines => Set<BudgetLine>();
    public DbSet<FinancialAuditLog> FinancialAuditLogs => Set<FinancialAuditLog>();
    public DbSet<SavedFinancialReport> SavedFinancialReports => Set<SavedFinancialReport>();

    // Phase 6: Expense Management
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseLineItem> ExpenseLineItems => Set<ExpenseLineItem>();
    public DbSet<ExpenseApproval> ExpenseApprovals => Set<ExpenseApproval>();
    public DbSet<ExpenseAttachment> ExpenseAttachments => Set<ExpenseAttachment>();
    public DbSet<ExpensePayment> ExpensePayments => Set<ExpensePayment>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<PurchaseOrderReceipt> PurchaseOrderReceipts => Set<PurchaseOrderReceipt>();
    public DbSet<PurchaseOrderReceiptLine> PurchaseOrderReceiptLines => Set<PurchaseOrderReceiptLine>();

    // Phase 7: Facility Management
    public DbSet<VenueOperatingSchedule> VenueOperatingSchedules => Set<VenueOperatingSchedule>();
    public DbSet<VenueHoliday> VenueHolidays => Set<VenueHoliday>();
    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<FacilityAvailability> FacilityAvailabilities => Set<FacilityAvailability>();
    public DbSet<FacilityPricing> FacilityPricings => Set<FacilityPricing>();
    public DbSet<FacilityBooking> FacilityBookings => Set<FacilityBooking>();
    public DbSet<FacilityMaintenance> FacilityMaintenances => Set<FacilityMaintenance>();
    public DbSet<FacilityBlockout> FacilityBlockouts => Set<FacilityBlockout>();

    // Phase 8: Equipment Management
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<EquipmentLoan> EquipmentLoans => Set<EquipmentLoan>();
    public DbSet<EquipmentMaintenance> EquipmentMaintenances => Set<EquipmentMaintenance>();
    public DbSet<EquipmentReservation> EquipmentReservations => Set<EquipmentReservation>();

    // Phase 9: Programs & Activities
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<ProgramSession> ProgramSessions => Set<ProgramSession>();
    public DbSet<ProgramEnrollment> ProgramEnrollments => Set<ProgramEnrollment>();
    public DbSet<ProgramInstructor> ProgramInstructors => Set<ProgramInstructor>();
    public DbSet<ProgramAttendance> ProgramAttendances => Set<ProgramAttendance>();
    public DbSet<MemberCertificate> MemberCertificates => Set<MemberCertificate>();

    // Phase 10: Events & Competitions Enhanced
    public DbSet<EventSeries> EventSeries => Set<EventSeries>();
    public DbSet<EventSession> EventSessions => Set<EventSession>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<CompetitionRound> CompetitionRounds => Set<CompetitionRound>();
    public DbSet<CompetitionTeam> CompetitionTeams => Set<CompetitionTeam>();
    public DbSet<CompetitionParticipant> CompetitionParticipants => Set<CompetitionParticipant>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchEvent> MatchEvents => Set<MatchEvent>();
    public DbSet<MatchLineup> MatchLineups => Set<MatchLineup>();
    public DbSet<CompetitionStanding> CompetitionStandings => Set<CompetitionStanding>();

    // System Configuration (global - not tenant-specific)
    public DbSet<SystemConfiguration> SystemConfigurations => Set<SystemConfiguration>();
    public DbSet<ConfigurationAuditLog> ConfigurationAuditLogs => Set<ConfigurationAuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply global query filters for multi-tenancy
        var currentTenantId = _tenantService?.CurrentTenantId;

        if (currentTenantId.HasValue && currentTenantId.Value != Guid.Empty)
        {
            builder.Entity<Member>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<FamilyMember>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MembershipType>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Membership>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Payment>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Session>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<RecurringSchedule>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<SessionBooking>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<RecurringBooking>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Waitlist>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Event>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EventTicket>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EventRSVP>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Venue>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MemberDocument>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MemberNote>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<CustomFieldDefinition>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<CommunicationTemplate>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EmailLog>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<BulkEmailCampaign>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<ClubAnalyticsSnapshot>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MembershipDiscount>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MembershipFreeze>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<GuestPass>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MembershipWaitlist>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Fee>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Invoice>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<PaymentPlan>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<PaymentInstallment>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MemberBalance>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<BalanceTransaction>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Refund>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<PaymentReminder>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<ChartOfAccount>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<JournalEntry>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<FiscalYear>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<FiscalPeriod>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<TaxRate>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<BankReconciliation>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Budget>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<FinancialAuditLog>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<SavedFinancialReport>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Vendor>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Expense>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<PurchaseOrder>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Facility>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<FacilityBooking>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Equipment>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EquipmentLoan>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EquipmentReservation>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Program>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<ProgramEnrollment>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<MemberCertificate>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EventSeries>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<EventRegistration>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Season>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<Competition>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<CompetitionTeam>().HasQueryFilter(e => e.ClubId == currentTenantId);
        }

        // Configure entity relationships
        ConfigureClub(builder);
        ConfigureMember(builder);
        ConfigureMembership(builder);
        ConfigureSession(builder);
        ConfigureEvent(builder);
        ConfigureVenue(builder);
        ConfigureFeesAndPayments(builder);
        ConfigureFinancialManagement(builder);
        ConfigureExpenseManagement(builder);
        ConfigureFacilityManagement(builder);
        ConfigureEquipmentManagement(builder);
        ConfigureProgramManagement(builder);
        ConfigureEventManagementEnhanced(builder);
        ConfigureCompetitionManagement(builder);
    }

    private void ConfigureClub(ModelBuilder builder)
    {
        builder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Slug).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);

            entity.HasOne(e => e.Settings)
                .WithOne(e => e.Club)
                .HasForeignKey<ClubSettings>(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ClubSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }

    private void ConfigureMember(ModelBuilder builder)
    {
        builder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Email });
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();

            entity.HasOne(e => e.Club)
                .WithMany(e => e.Members)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PrimaryMember)
                .WithMany()
                .HasForeignKey(e => e.PrimaryMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.PrimaryMember)
                .WithMany(e => e.FamilyMembers)
                .HasForeignKey(e => e.PrimaryMemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<MemberDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();

            entity.HasOne(e => e.Member)
                .WithMany(e => e.Documents)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureMembership(ModelBuilder builder)
    {
        builder.Entity<MembershipType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.ColorCode).HasMaxLength(20);
            entity.Property(e => e.IconName).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);

            // Pricing - all decimal fields need precision
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            entity.Property(e => e.WeeklyFee).HasPrecision(18, 2);
            entity.Property(e => e.FortnightlyFee).HasPrecision(18, 2);
            entity.Property(e => e.MonthlyFee).HasPrecision(18, 2);
            entity.Property(e => e.QuarterlyFee).HasPrecision(18, 2);
            entity.Property(e => e.BiannualFee).HasPrecision(18, 2);
            entity.Property(e => e.AnnualFee).HasPrecision(18, 2);
            entity.Property(e => e.LifetimeFee).HasPrecision(18, 2);
            entity.Property(e => e.SessionFee).HasPrecision(18, 2);
            entity.Property(e => e.JoiningFee).HasPrecision(18, 2);
            entity.Property(e => e.AdminFee).HasPrecision(18, 2);
            entity.Property(e => e.AdditionalMemberFee).HasPrecision(18, 2);
            entity.Property(e => e.PromotionalPrice).HasPrecision(18, 2);
            entity.Property(e => e.FreezeFeePerMonth).HasPrecision(18, 2);
            entity.Property(e => e.EarlyCancellationFee).HasPrecision(18, 2);
            entity.Property(e => e.NewPriceAfterIncrease).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.MembershipTypes)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.DiscountCode).HasMaxLength(50);

            // Pricing fields
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            entity.Property(e => e.CurrentPrice).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.AmountDue).HasPrecision(18, 2);
            entity.Property(e => e.CancellationFeeCharged).HasPrecision(18, 2);

            entity.HasOne(e => e.Member)
                .WithMany(e => e.Memberships)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MembershipType)
                .WithMany(e => e.Memberships)
                .HasForeignKey(e => e.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PreviousMembershipType)
                .WithMany()
                .HasForeignKey(e => e.PreviousMembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.ReceiptNumber).HasMaxLength(50);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Membership)
                .WithMany(e => e.Payments)
                .HasForeignKey(e => e.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MembershipDiscount configuration
        builder.Entity<MembershipDiscount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PromoCode).HasMaxLength(50);
            entity.Property(e => e.CorporatePartnerName).HasMaxLength(200);
            entity.Property(e => e.CorporatePartnerCode).HasMaxLength(50);

            entity.Property(e => e.PercentageOff).HasPrecision(5, 2);
            entity.Property(e => e.FixedAmountOff).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.PromoCode }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MembershipType)
                .WithMany(e => e.Discounts)
                .HasForeignKey(e => e.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MembershipFreeze configuration
        builder.Entity<MembershipFreeze>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FeeDuringFreeze).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.MembershipId });

            entity.HasOne(e => e.Membership)
                .WithMany(e => e.FreezeHistory)
                .HasForeignKey(e => e.MembershipId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // GuestPass configuration
        builder.Entity<GuestPass>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PassCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.GuestFirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.GuestLastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.GuestEmail).HasMaxLength(255);
            entity.Property(e => e.GuestPhone).HasMaxLength(50);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DiscountApplied).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.PassCode }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.HostMember)
                .WithMany()
                .HasForeignKey(e => e.HostMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ConvertedMember)
                .WithMany()
                .HasForeignKey(e => e.ConvertedMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MembershipWaitlist configuration
        builder.Entity<MembershipWaitlist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.ReferredBy).HasMaxLength(200);

            entity.HasIndex(e => new { e.ClubId, e.MembershipTypeId, e.Position });

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MembershipType)
                .WithMany(e => e.Waitlist)
                .HasForeignKey(e => e.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ReferringMember)
                .WithMany()
                .HasForeignKey(e => e.ReferringMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedMembership)
                .WithMany()
                .HasForeignKey(e => e.CreatedMembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureSession(ModelBuilder builder)
    {
        builder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.SessionFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Venue)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.RecurringSchedule)
                .WithMany(e => e.GeneratedSessions)
                .HasForeignKey(e => e.RecurringScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<RecurringSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.SessionFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<SessionBooking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.SessionId, e.MemberId, e.FamilyMemberId });

            entity.HasOne(e => e.Session)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(e => e.SessionBookings)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FamilyMember)
                .WithMany(e => e.SessionBookings)
                .HasForeignKey(e => e.FamilyMemberId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<RecurringBooking>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.RecurringSchedule)
                .WithMany(e => e.RecurringBookings)
                .HasForeignKey(e => e.RecurringScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Waitlist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.SessionId, e.Position });

            entity.HasOne(e => e.Session)
                .WithMany(e => e.WaitlistEntries)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureEvent(ModelBuilder builder)
    {
        builder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.TicketPrice).HasPrecision(18, 2);
            entity.Property(e => e.MemberTicketPrice).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Venue)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<EventTicket>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(e => e.EventTickets)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<EventRSVP>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Event)
                .WithMany(e => e.RSVPs)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureVenue(ModelBuilder builder)
    {
        builder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.County).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.WhatThreeWords).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Website).HasMaxLength(255);
            entity.Property(e => e.ContactName).HasMaxLength(100);
            entity.Property(e => e.AreaUnit).HasMaxLength(20);
            entity.Property(e => e.OwnershipType).HasMaxLength(50);
            entity.Property(e => e.LandlordName).HasMaxLength(200);
            entity.Property(e => e.LandlordContact).HasMaxLength(200);
            entity.Property(e => e.TimeZone).HasMaxLength(50);
            entity.Property(e => e.InsuranceProvider).HasMaxLength(200);
            entity.Property(e => e.InsurancePolicyNumber).HasMaxLength(100);
            entity.Property(e => e.EmergencyContact).HasMaxLength(200);
            entity.Property(e => e.NearestHospital).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
            entity.Property(e => e.TotalArea).HasPrecision(10, 2);
            entity.Property(e => e.MonthlyRent).HasPrecision(18, 2);
            entity.Property(e => e.CancellationFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.Venues)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<VenueOperatingSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeasonName).HasMaxLength(100);

            entity.HasOne(e => e.Venue)
                .WithMany(v => v.OperatingSchedules)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<VenueHoliday>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();

            entity.HasOne(e => e.Venue)
                .WithMany(v => v.Holidays)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ToEmail).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Subject).HasMaxLength(500);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<BulkEmailCampaign>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Subject).HasMaxLength(500);
        });

        builder.Entity<ClubAnalyticsSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.SnapshotDate });
            entity.Property(e => e.TotalRevenueThisMonth).HasPrecision(18, 2);
            entity.Property(e => e.TotalRevenueThisYear).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingPayments).HasPrecision(18, 2);
            entity.Property(e => e.AverageAttendanceRate).HasPrecision(5, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MemberNote configuration
        builder.Entity<MemberNote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.HasIndex(e => new { e.ClubId, e.MemberId });

            entity.HasOne(e => e.Member)
                .WithMany(m => m.Notes)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CustomFieldDefinition configuration
        builder.Entity<CustomFieldDefinition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Label).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => new { e.ClubId, e.Name }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany(c => c.CustomFields)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CommunicationTemplate configuration
        builder.Entity<CommunicationTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Subject).HasMaxLength(500);
            entity.HasIndex(e => new { e.ClubId, e.Category });

            entity.HasOne(e => e.Club)
                .WithMany(c => c.CommunicationTemplates)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Member additional configurations
        builder.Entity<Member>(entity =>
        {
            entity.Property(e => e.MemberNumber).HasMaxLength(50);
            entity.HasIndex(e => new { e.ClubId, e.MemberNumber }).IsUnique();

            entity.HasOne(e => e.ReferredByMember)
                .WithMany()
                .HasForeignKey(e => e.ReferredByMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureFeesAndPayments(ModelBuilder builder)
    {
        // Fee configuration
        builder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.GLAccountCode).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);
            entity.Property(e => e.TaxCode).HasMaxLength(20);

            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.MinAmount).HasPrecision(18, 2);
            entity.Property(e => e.MaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
            entity.Property(e => e.LateFeeAmount).HasPrecision(18, 2);
            entity.Property(e => e.LateFeePercentage).HasPrecision(5, 2);
            entity.Property(e => e.EarlyPaymentDiscountPercent).HasPrecision(5, 2);

            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Invoice configuration
        builder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.TaxNumber).HasMaxLength(50);
            entity.Property(e => e.BillingName).HasMaxLength(200);
            entity.Property(e => e.BillingAddress).HasMaxLength(500);
            entity.Property(e => e.BillingCity).HasMaxLength(100);
            entity.Property(e => e.BillingPostcode).HasMaxLength(20);
            entity.Property(e => e.BillingCountry).HasMaxLength(100);
            entity.Property(e => e.BillingEmail).HasMaxLength(255);
            entity.Property(e => e.CorporateName).HasMaxLength(200);

            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
            entity.Property(e => e.BalanceDue).HasPrecision(18, 2);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
            entity.Property(e => e.LateFeeApplied).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.InvoiceNumber }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PrimaryMember)
                .WithMany()
                .HasForeignKey(e => e.PrimaryMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InvoiceLineItem configuration
        builder.Entity<InvoiceLineItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ServicePeriod).HasMaxLength(100);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(50);
            entity.Property(e => e.GLAccountCode).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);
            entity.Property(e => e.TaxCode).HasMaxLength(20);

            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.DiscountPercent).HasPrecision(5, 2);
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.LineItems)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Fee)
                .WithMany()
                .HasForeignKey(e => e.FeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PaymentPlan configuration
        builder.Entity<PaymentPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PlanName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PlanReference).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.DefaultPaymentMethod).HasMaxLength(100);

            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
            entity.Property(e => e.RemainingAmount).HasPrecision(18, 2);
            entity.Property(e => e.SetupFee).HasPrecision(18, 2);
            entity.Property(e => e.InterestRate).HasPrecision(5, 2);
            entity.Property(e => e.TotalInterest).HasPrecision(18, 2);
            entity.Property(e => e.InstallmentAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalInstallmentAmount).HasPrecision(18, 2);
            entity.Property(e => e.LateFeePerMissed).HasPrecision(18, 2);
            entity.Property(e => e.TotalLateFees).HasPrecision(18, 2);
            entity.Property(e => e.EarlyCancellationFee).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.PlanReference }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Membership)
                .WithMany()
                .HasForeignKey(e => e.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PaymentInstallment configuration
        builder.Entity<PaymentInstallment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionReference).HasMaxLength(100);

            entity.Property(e => e.AmountDue).HasPrecision(18, 2);
            entity.Property(e => e.InterestAmount).HasPrecision(18, 2);
            entity.Property(e => e.LateFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalDue).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.BalanceDue).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.PaymentPlanId, e.InstallmentNumber });

            entity.HasOne(e => e.PaymentPlan)
                .WithMany(p => p.Installments)
                .HasForeignKey(e => e.PaymentPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MemberBalance configuration
        builder.Entity<MemberBalance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentRating).HasMaxLength(20);
            entity.Property(e => e.DefaultPaymentMethodId).HasMaxLength(100);
            entity.Property(e => e.DefaultPaymentMethodType).HasMaxLength(50);
            entity.Property(e => e.DefaultPaymentMethodLast4).HasMaxLength(4);

            entity.Property(e => e.CreditBalance).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingBalance).HasPrecision(18, 2);
            entity.Property(e => e.NetBalance).HasPrecision(18, 2);
            entity.Property(e => e.Current).HasPrecision(18, 2);
            entity.Property(e => e.Overdue30).HasPrecision(18, 2);
            entity.Property(e => e.Overdue60).HasPrecision(18, 2);
            entity.Property(e => e.Overdue90).HasPrecision(18, 2);
            entity.Property(e => e.Overdue120Plus).HasPrecision(18, 2);
            entity.Property(e => e.ExpiringCredit).HasPrecision(18, 2);
            entity.Property(e => e.TotalPaidAllTime).HasPrecision(18, 2);
            entity.Property(e => e.TotalPaidThisYear).HasPrecision(18, 2);
            entity.Property(e => e.TotalChargedAllTime).HasPrecision(18, 2);
            entity.Property(e => e.TotalChargedThisYear).HasPrecision(18, 2);
            entity.Property(e => e.TotalRefundedAllTime).HasPrecision(18, 2);
            entity.Property(e => e.TotalWrittenOff).HasPrecision(18, 2);
            entity.Property(e => e.LastPaymentAmount).HasPrecision(18, 2);
            entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
            entity.Property(e => e.AveragePaymentDelay).HasPrecision(5, 2);

            entity.HasIndex(e => new { e.ClubId, e.MemberId }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentPlan)
                .WithMany()
                .HasForeignKey(e => e.PaymentPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // BalanceTransaction configuration
        builder.Entity<BalanceTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Source).HasMaxLength(50);
            entity.Property(e => e.ProcessedBy).HasMaxLength(200);

            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.BalanceBefore).HasPrecision(18, 2);
            entity.Property(e => e.BalanceAfter).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.MemberId, e.TransactionDate });

            entity.HasOne(e => e.MemberBalance)
                .WithMany(b => b.Transactions)
                .HasForeignKey(e => e.MemberBalanceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Refund)
                .WithMany()
                .HasForeignKey(e => e.RefundId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Refund configuration
        builder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RefundNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ReasonDetails).HasMaxLength(1000);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.RefundToAccount).HasMaxLength(100);
            entity.Property(e => e.StripeRefundId).HasMaxLength(100);
            entity.Property(e => e.PayPalRefundId).HasMaxLength(100);
            entity.Property(e => e.BankTransferReference).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.RequestedBy).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.ProcessedBy).HasMaxLength(200);
            entity.Property(e => e.RejectedBy).HasMaxLength(200);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);

            entity.Property(e => e.OriginalPaymentAmount).HasPrecision(18, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.RefundNumber }).IsUnique();

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Payment)
                .WithMany(p => p.Refunds)
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BalanceTransaction)
                .WithMany()
                .HasForeignKey(e => e.BalanceTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PaymentReminder configuration
        builder.Entity<PaymentReminder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Channel).HasMaxLength(20);
            entity.Property(e => e.ToEmail).HasMaxLength(255);
            entity.Property(e => e.ToPhone).HasMaxLength(50);
            entity.Property(e => e.Subject).HasMaxLength(500);
            entity.Property(e => e.EscalationAction).HasMaxLength(100);

            entity.Property(e => e.AmountDue).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.ClubId, e.MemberId, e.ScheduledDate });

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentPlan)
                .WithMany()
                .HasForeignKey(e => e.PaymentPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Installment)
                .WithMany()
                .HasForeignKey(e => e.InstallmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Membership)
                .WithMany()
                .HasForeignKey(e => e.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Template)
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Enhanced Payment configuration
        builder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
            entity.Property(e => e.ExternalReference).HasMaxLength(100);
            entity.Property(e => e.StripeChargeId).HasMaxLength(100);
            entity.Property(e => e.PayPalOrderId).HasMaxLength(100);
            entity.Property(e => e.GoCardlessPaymentId).HasMaxLength(100);
            entity.Property(e => e.BankTransferReference).HasMaxLength(100);
            entity.Property(e => e.CardLast4).HasMaxLength(4);
            entity.Property(e => e.CardBrand).HasMaxLength(20);
            entity.Property(e => e.CardExpiryMonth).HasMaxLength(2);
            entity.Property(e => e.CardExpiryYear).HasMaxLength(4);
            entity.Property(e => e.BankAccountLast4).HasMaxLength(4);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.FailureCode).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.OriginalAmount).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TipAmount).HasPrecision(18, 2);
            entity.Property(e => e.ProcessingFee).HasPrecision(18, 2);
            entity.Property(e => e.NetAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmountOwed).HasPrecision(18, 2);
            entity.Property(e => e.RemainingBalance).HasPrecision(18, 2);
            entity.Property(e => e.RefundedAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentPlan)
                .WithMany()
                .HasForeignKey(e => e.PaymentPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Installment)
                .WithMany()
                .HasForeignKey(e => e.InstallmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Fee)
                .WithMany(f => f.Payments)
                .HasForeignKey(e => e.FeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureFinancialManagement(ModelBuilder builder)
    {
        // Chart of Accounts
        builder.Entity<ChartOfAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.AccountCode }).IsUnique();
            entity.Property(e => e.AccountCode).HasMaxLength(20).IsRequired();
            entity.Property(e => e.AccountName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.FullPath).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.SortCode).HasMaxLength(20);
            entity.Property(e => e.IBAN).HasMaxLength(50);
            entity.Property(e => e.SwiftCode).HasMaxLength(20);
            entity.Property(e => e.LockedBy).HasMaxLength(200);
            entity.Property(e => e.DisplayColor).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
            entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
            entity.Property(e => e.YearToDateBalance).HasPrecision(18, 2);
            entity.Property(e => e.LastYearBalance).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ParentAccount)
                .WithMany(e => e.ChildAccounts)
                .HasForeignKey(e => e.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DefaultTaxRate)
                .WithMany()
                .HasForeignKey(e => e.DefaultTaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Journal Entry
        builder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.EntryNumber }).IsUnique();
            entity.Property(e => e.EntryNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.SourceType).HasMaxLength(50);
            entity.Property(e => e.SourceReference).HasMaxLength(100);
            entity.Property(e => e.PreparedBy).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.PostedBy).HasMaxLength(200);
            entity.Property(e => e.VoidedBy).HasMaxLength(200);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.TotalDebit).HasPrecision(18, 2);
            entity.Property(e => e.TotalCredit).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FiscalPeriod)
                .WithMany(e => e.JournalEntries)
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ReversedFrom)
                .WithOne()
                .HasForeignKey<JournalEntry>(e => e.ReversedFromId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ReversedBy)
                .WithOne()
                .HasForeignKey<JournalEntry>(e => e.ReversedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Journal Entry Line
        builder.Entity<JournalEntryLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Project).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);
            entity.Property(e => e.ExternalReference).HasMaxLength(100);

            entity.Property(e => e.DebitAmount).HasPrecision(18, 2);
            entity.Property(e => e.CreditAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.JournalEntry)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Account)
                .WithMany(e => e.JournalEntryLines)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TaxRate)
                .WithMany()
                .HasForeignKey(e => e.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Fiscal Year
        builder.Entity<FiscalYear>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.ClosedBy).HasMaxLength(200);
            entity.Property(e => e.LockedBy).HasMaxLength(200);
            entity.Property(e => e.OpeningBalancesPostedBy).HasMaxLength(200);
            entity.Property(e => e.ClosingEntriesPostedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.TotalRevenue).HasPrecision(18, 2);
            entity.Property(e => e.TotalExpenses).HasPrecision(18, 2);
            entity.Property(e => e.NetIncome).HasPrecision(18, 2);
            entity.Property(e => e.TotalAssets).HasPrecision(18, 2);
            entity.Property(e => e.TotalLiabilities).HasPrecision(18, 2);
            entity.Property(e => e.TotalEquity).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Fiscal Period
        builder.Entity<FiscalPeriod>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.ClosedBy).HasMaxLength(200);
            entity.Property(e => e.LockedBy).HasMaxLength(200);
            entity.Property(e => e.ReopenedBy).HasMaxLength(200);
            entity.Property(e => e.ReconciledBy).HasMaxLength(200);

            entity.Property(e => e.TotalRevenue).HasPrecision(18, 2);
            entity.Property(e => e.TotalExpenses).HasPrecision(18, 2);
            entity.Property(e => e.NetIncome).HasPrecision(18, 2);
            entity.Property(e => e.TotalDebits).HasPrecision(18, 2);
            entity.Property(e => e.TotalCredits).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FiscalYear)
                .WithMany(e => e.Periods)
                .HasForeignKey(e => e.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Tax Rate
        builder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TaxAuthorityCode).HasMaxLength(50);
            entity.Property(e => e.ReportingCategory).HasMaxLength(50);
            entity.Property(e => e.DisplayColor).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Rate).HasPrecision(5, 2);
            entity.Property(e => e.EffectiveRate).HasPrecision(5, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CollectedAccount)
                .WithMany()
                .HasForeignKey(e => e.CollectedAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaidAccount)
                .WithMany()
                .HasForeignKey(e => e.PaidAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Bank Reconciliation
        builder.Entity<BankReconciliation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StatementReference).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.StartedBy).HasMaxLength(200);
            entity.Property(e => e.CompletedBy).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.ImportedFileName).HasMaxLength(255);
            entity.Property(e => e.ImportFormat).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
            entity.Property(e => e.ClosingBalance).HasPrecision(18, 2);
            entity.Property(e => e.BookOpeningBalance).HasPrecision(18, 2);
            entity.Property(e => e.BookClosingBalance).HasPrecision(18, 2);
            entity.Property(e => e.TotalDeposits).HasPrecision(18, 2);
            entity.Property(e => e.TotalWithdrawals).HasPrecision(18, 2);
            entity.Property(e => e.TotalBankFees).HasPrecision(18, 2);
            entity.Property(e => e.TotalInterest).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingDeposits).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingWithdrawals).HasPrecision(18, 2);
            entity.Property(e => e.Adjustments).HasPrecision(18, 2);
            entity.Property(e => e.Variance).HasPrecision(18, 2);
            entity.Property(e => e.AdjustedBookBalance).HasPrecision(18, 2);
            entity.Property(e => e.ReconciledPercentage).HasPrecision(5, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.BankAccount)
                .WithMany()
                .HasForeignKey(e => e.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Bank Reconciliation Line
        builder.Entity<BankReconciliationLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.Payee).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.BankReference).HasMaxLength(100);
            entity.Property(e => e.BankDescription).HasMaxLength(500);
            entity.Property(e => e.BankTransactionType).HasMaxLength(50);
            entity.Property(e => e.ReconciledBy).HasMaxLength(200);
            entity.Property(e => e.MatchType).HasMaxLength(20);

            entity.Property(e => e.Deposit).HasPrecision(18, 2);
            entity.Property(e => e.Withdrawal).HasPrecision(18, 2);
            entity.Property(e => e.RunningBalance).HasPrecision(18, 2);
            entity.Property(e => e.MatchConfidence).HasPrecision(5, 2);

            entity.HasOne(e => e.BankReconciliation)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.BankReconciliationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.JournalEntryLine)
                .WithMany()
                .HasForeignKey(e => e.JournalEntryLineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Budget
        builder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PreparedBy).HasMaxLength(200);
            entity.Property(e => e.ReviewedBy).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.RejectedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.TotalBudgetedRevenue).HasPrecision(18, 2);
            entity.Property(e => e.TotalBudgetedExpenses).HasPrecision(18, 2);
            entity.Property(e => e.TotalBudgetedProfit).HasPrecision(18, 2);
            entity.Property(e => e.TotalActualRevenue).HasPrecision(18, 2);
            entity.Property(e => e.TotalActualExpenses).HasPrecision(18, 2);
            entity.Property(e => e.TotalActualProfit).HasPrecision(18, 2);
            entity.Property(e => e.RevenueVariance).HasPrecision(18, 2);
            entity.Property(e => e.ExpenseVariance).HasPrecision(18, 2);
            entity.Property(e => e.ProfitVariance).HasPrecision(18, 2);
            entity.Property(e => e.RevenueVariancePercent).HasPrecision(5, 2);
            entity.Property(e => e.ExpenseVariancePercent).HasPrecision(5, 2);
            entity.Property(e => e.OverBudgetThreshold).HasPrecision(5, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FiscalYear)
                .WithMany(e => e.Budgets)
                .HasForeignKey(e => e.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PreviousVersion)
                .WithMany()
                .HasForeignKey(e => e.PreviousVersionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Budget Line
        builder.Entity<BudgetLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.SubCategory).HasMaxLength(100);
            entity.Property(e => e.CalculationMethod).HasMaxLength(50);

            // Period amounts - all need precision
            entity.Property(e => e.Period1Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period2Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period3Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period4Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period5Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period6Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period7Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period8Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period9Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period10Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period11Amount).HasPrecision(18, 2);
            entity.Property(e => e.Period12Amount).HasPrecision(18, 2);
            entity.Property(e => e.TotalBudgeted).HasPrecision(18, 2);

            // Period actuals
            entity.Property(e => e.Period1Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period2Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period3Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period4Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period5Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period6Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period7Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period8Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period9Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period10Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period11Actual).HasPrecision(18, 2);
            entity.Property(e => e.Period12Actual).HasPrecision(18, 2);
            entity.Property(e => e.TotalActual).HasPrecision(18, 2);

            entity.Property(e => e.Variance).HasPrecision(18, 2);
            entity.Property(e => e.VariancePercent).HasPrecision(5, 2);
            entity.Property(e => e.PercentOfBase).HasPrecision(5, 2);

            entity.HasOne(e => e.Budget)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BasedOnLine)
                .WithMany()
                .HasForeignKey(e => e.BasedOnLineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Financial Audit Log
        builder.Entity<FinancialAuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.ActionDate });
            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.Property(e => e.ActionBy).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ActionByIpAddress).HasMaxLength(50);
            entity.Property(e => e.EntityType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityReference).HasMaxLength(100);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(100);
            entity.Property(e => e.ApproverRole).HasMaxLength(50);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.RequestId).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.SubCategory).HasMaxLength(50);
            entity.Property(e => e.ReviewedBy).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);

            entity.Property(e => e.AmountBefore).HasPrecision(18, 2);
            entity.Property(e => e.AmountAfter).HasPrecision(18, 2);
            entity.Property(e => e.AmountChange).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FiscalPeriod)
                .WithMany()
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FiscalYear)
                .WithMany()
                .HasForeignKey(e => e.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentAuditLog)
                .WithMany()
                .HasForeignKey(e => e.ParentAuditLogId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Saved Financial Report
        builder.Entity<SavedFinancialReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DateRangeType).HasMaxLength(50);
            entity.Property(e => e.ChartType).HasMaxLength(50);
            entity.Property(e => e.DisplayFormat).HasMaxLength(50);
            entity.Property(e => e.Schedule).HasMaxLength(100);
            entity.Property(e => e.ExportFormat).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureExpenseManagement(ModelBuilder builder)
    {
        // Vendor
        builder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.VendorNumber }).IsUnique();
            entity.Property(e => e.VendorNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.TradingName).HasMaxLength(200);
            entity.Property(e => e.ContactName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Mobile).HasMaxLength(30);
            entity.Property(e => e.Website).HasMaxLength(255);
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.County).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.TaxId).HasMaxLength(50);
            entity.Property(e => e.CompanyNumber).HasMaxLength(50);
            entity.Property(e => e.TaxClassification).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BankAccountName).HasMaxLength(100);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.SortCode).HasMaxLength(20);
            entity.Property(e => e.IBAN).HasMaxLength(50);
            entity.Property(e => e.SwiftCode).HasMaxLength(20);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.AccountManager).HasMaxLength(100);
            entity.Property(e => e.PreferredCommunication).HasMaxLength(50);
            entity.Property(e => e.Rating).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
            entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
            entity.Property(e => e.AveragePaymentDays).HasPrecision(5, 2);
            entity.Property(e => e.TotalSpend).HasPrecision(18, 2);
            entity.Property(e => e.TotalSpendYTD).HasPrecision(18, 2);
            entity.Property(e => e.POApprovalThreshold).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.DefaultExpenseAccount)
                .WithMany()
                .HasForeignKey(e => e.DefaultExpenseAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DefaultTaxRate)
                .WithMany()
                .HasForeignKey(e => e.DefaultTaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Expense
        builder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.ExpenseNumber }).IsUnique();
            entity.Property(e => e.ExpenseNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.SubCategory).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.OriginalCurrency).HasMaxLength(3);
            entity.Property(e => e.PaidBy).HasMaxLength(200);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Project).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);
            entity.Property(e => e.PostedBy).HasMaxLength(200);
            entity.Property(e => e.CurrentApprover).HasMaxLength(200);
            entity.Property(e => e.RecurrencePattern).HasMaxLength(100);
            entity.Property(e => e.VendorBillNumber).HasMaxLength(100);
            entity.Property(e => e.VoidedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);
            entity.Property(e => e.SubmittedBy).HasMaxLength(200);

            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
            entity.Property(e => e.BalanceDue).HasPrecision(18, 2);
            entity.Property(e => e.ExchangeRate).HasPrecision(10, 6);
            entity.Property(e => e.OriginalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ApprovalThreshold).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Vendor)
                .WithMany(v => v.Expenses)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DefaultAccount)
                .WithMany()
                .HasForeignKey(e => e.DefaultAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FiscalPeriod)
                .WithMany()
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.JournalEntry)
                .WithMany()
                .HasForeignKey(e => e.JournalEntryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentExpense)
                .WithMany()
                .HasForeignKey(e => e.ParentExpenseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Expense Line Item
        builder.Entity<ExpenseLineItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Project).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);

            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.LineItems)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TaxRate)
                .WithMany()
                .HasForeignKey(e => e.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PurchaseOrderLine)
                .WithMany(p => p.ExpenseLineItems)
                .HasForeignKey(e => e.PurchaseOrderLineId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BillToMember)
                .WithMany()
                .HasForeignKey(e => e.BillToMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Expense Approval
        builder.Entity<ExpenseApproval>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApproverUserId).HasMaxLength(200);
            entity.Property(e => e.ApproverName).HasMaxLength(200);
            entity.Property(e => e.ApproverRole).HasMaxLength(50);
            entity.Property(e => e.EscalatedTo).HasMaxLength(200);

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.Approvals)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Expense Attachment
        builder.Entity<ExpenseAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.OriginalFileName).HasMaxLength(255);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.AttachmentType).HasMaxLength(50);
            entity.Property(e => e.UploadedBy).HasMaxLength(200);

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.Attachments)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Expense Payment
        builder.Entity<ExpensePayment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionReference).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.Amount).HasPrecision(18, 2);

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.Payments)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BankAccount)
                .WithMany()
                .HasForeignKey(e => e.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Purchase Order
        builder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.PONumber }).IsUnique();
            entity.Property(e => e.PONumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.SentTo).HasMaxLength(255);
            entity.Property(e => e.SentVia).HasMaxLength(50);
            entity.Property(e => e.ShipToName).HasMaxLength(200);
            entity.Property(e => e.ShipToAddress).HasMaxLength(500);
            entity.Property(e => e.ShipToCity).HasMaxLength(100);
            entity.Property(e => e.ShipToPostCode).HasMaxLength(20);
            entity.Property(e => e.ShippingMethod).HasMaxLength(100);
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Project).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);
            entity.Property(e => e.CurrentApprover).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.RejectedBy).HasMaxLength(200);
            entity.Property(e => e.VendorReference).HasMaxLength(100);
            entity.Property(e => e.ClosedBy).HasMaxLength(200);
            entity.Property(e => e.CancelledBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.ShippingAmount).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ReceivedAmount).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingAmount).HasPrecision(18, 2);
            entity.Property(e => e.BilledAmount).HasPrecision(18, 2);
            entity.Property(e => e.UnbilledAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Vendor)
                .WithMany(v => v.PurchaseOrders)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FiscalPeriod)
                .WithMany()
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Purchase Order Line
        builder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ItemCode).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Project).HasMaxLength(50);
            entity.Property(e => e.CostCenter).HasMaxLength(50);

            entity.Property(e => e.QuantityOrdered).HasPrecision(18, 4);
            entity.Property(e => e.QuantityReceived).HasPrecision(18, 4);
            entity.Property(e => e.QuantityOutstanding).HasPrecision(18, 4);
            entity.Property(e => e.QuantityBilled).HasPrecision(18, 4);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.DiscountPercent).HasPrecision(5, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany(p => p.Lines)
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TaxRate)
                .WithMany()
                .HasForeignKey(e => e.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Purchase Order Receipt
        builder.Entity<PurchaseOrderReceipt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ReceiptNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ReceivedBy).HasMaxLength(200);
            entity.Property(e => e.DeliveryNote).HasMaxLength(100);
            entity.Property(e => e.CarrierName).HasMaxLength(100);
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.QualityCheckedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany(p => p.Receipts)
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Purchase Order Receipt Line
        builder.Entity<PurchaseOrderReceiptLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Condition).HasMaxLength(50);
            entity.Property(e => e.StorageLocation).HasMaxLength(100);
            entity.Property(e => e.BatchNumber).HasMaxLength(50);

            entity.Property(e => e.QuantityReceived).HasPrecision(18, 4);
            entity.Property(e => e.QuantityAccepted).HasPrecision(18, 4);
            entity.Property(e => e.QuantityRejected).HasPrecision(18, 4);

            entity.HasOne(e => e.Receipt)
                .WithMany(r => r.Lines)
                .HasForeignKey(e => e.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PurchaseOrderLine)
                .WithMany()
                .HasForeignKey(e => e.PurchaseOrderLineId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureFacilityManagement(ModelBuilder builder)
    {
        // Facility
        builder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DimensionUnit).HasMaxLength(20);
            entity.Property(e => e.SurfaceType).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Length).HasPrecision(10, 2);
            entity.Property(e => e.Width).HasPrecision(10, 2);
            entity.Property(e => e.Area).HasPrecision(10, 2);
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            entity.Property(e => e.MemberPrice).HasPrecision(18, 2);
            entity.Property(e => e.NonMemberPrice).HasPrecision(18, 2);
            entity.Property(e => e.PeakPrice).HasPrecision(18, 2);
            entity.Property(e => e.OffPeakPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalRevenue).HasPrecision(18, 2);

            entity.HasOne(e => e.Venue)
                .WithMany(v => v.Facilities)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentFacility)
                .WithMany(f => f.SubFacilities)
                .HasForeignKey(e => e.ParentFacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DefaultTaxRate)
                .WithMany()
                .HasForeignKey(e => e.DefaultTaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Facility Availability
        builder.Entity<FacilityAvailability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeasonName).HasMaxLength(100);

            entity.HasOne(e => e.Facility)
                .WithMany(f => f.AvailabilitySchedules)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Facility Pricing
        builder.Entity<FacilityPricing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(e => e.Facility)
                .WithMany(f => f.PricingRules)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MembershipType)
                .WithMany()
                .HasForeignKey(e => e.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Facility Booking
        builder.Entity<FacilityBooking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.BookingNumber }).IsUnique();
            entity.Property(e => e.BookingNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.GuestName).HasMaxLength(200);
            entity.Property(e => e.GuestEmail).HasMaxLength(255);
            entity.Property(e => e.GuestPhone).HasMaxLength(30);
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.CheckedInBy).HasMaxLength(200);
            entity.Property(e => e.CancelledBy).HasMaxLength(200);
            entity.Property(e => e.RecurrencePattern).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.BookingSource).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Facility)
                .WithMany(f => f.Bookings)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Session)
                .WithMany()
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Facility Maintenance
        builder.Entity<FacilityMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.AssignedTo).HasMaxLength(200);
            entity.Property(e => e.AssignedTeam).HasMaxLength(100);
            entity.Property(e => e.CompletedBy).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.InspectedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
            entity.Property(e => e.ActualCost).HasPrecision(18, 2);

            entity.HasOne(e => e.Facility)
                .WithMany(f => f.MaintenanceRecords)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Expense)
                .WithMany()
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany()
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentMaintenance)
                .WithMany(m => m.FollowUpMaintenance)
                .HasForeignKey(e => e.ParentMaintenanceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Facility Blockout
        builder.Entity<FacilityBlockout>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RecurrencePattern).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Facility)
                .WithMany(f => f.Blockouts)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Maintenance)
                .WithMany()
                .HasForeignKey(e => e.MaintenanceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureEquipmentManagement(ModelBuilder builder)
    {
        // Equipment
        builder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.BarCode).HasMaxLength(100);
            entity.Property(e => e.SubCategory).HasMaxLength(100);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Size).HasMaxLength(50);
            entity.Property(e => e.WeightUnit).HasMaxLength(20);
            entity.Property(e => e.Dimensions).HasMaxLength(100);
            entity.Property(e => e.StorageLocation).HasMaxLength(200);
            entity.Property(e => e.Shelf).HasMaxLength(50);
            entity.Property(e => e.Bin).HasMaxLength(50);
            entity.Property(e => e.PurchaseCurrency).HasMaxLength(3);
            entity.Property(e => e.SupplierName).HasMaxLength(200);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.WarrantyProvider).HasMaxLength(200);
            entity.Property(e => e.InsurancePolicy).HasMaxLength(100);
            entity.Property(e => e.InspectionFrequency).HasMaxLength(50);
            entity.Property(e => e.RetiredBy).HasMaxLength(200);
            entity.Property(e => e.DisposalMethod).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Weight).HasPrecision(10, 2);
            entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
            entity.Property(e => e.CurrentValue).HasPrecision(18, 2);
            entity.Property(e => e.ReplacementCost).HasPrecision(18, 2);
            entity.Property(e => e.DepreciationRate).HasPrecision(5, 2);
            entity.Property(e => e.SalvageValue).HasPrecision(18, 2);
            entity.Property(e => e.InsuredValue).HasPrecision(18, 2);
            entity.Property(e => e.DepositAmount).HasPrecision(18, 2);
            entity.Property(e => e.DailyLoanFee).HasPrecision(18, 2);
            entity.Property(e => e.LateFeePerDay).HasPrecision(18, 2);
            entity.Property(e => e.TotalLoanRevenue).HasPrecision(18, 2);
            entity.Property(e => e.DisposalValue).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany()
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Equipment Loan
        builder.Entity<EquipmentLoan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.LoanNumber }).IsUnique();
            entity.Property(e => e.LoanNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.DepositPaymentRef).HasMaxLength(100);
            entity.Property(e => e.CollectedBy).HasMaxLength(200);
            entity.Property(e => e.ReturnedTo).HasMaxLength(200);
            entity.Property(e => e.ReturnLocation).HasMaxLength(200);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.DepositAmount).HasPrecision(18, 2);
            entity.Property(e => e.DepositDeduction).HasPrecision(18, 2);
            entity.Property(e => e.DailyFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalFee).HasPrecision(18, 2);
            entity.Property(e => e.LateFee).HasPrecision(18, 2);
            entity.Property(e => e.DamageFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.Loans)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Session)
                .WithMany()
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Equipment Maintenance
        builder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.AssignedTo).HasMaxLength(200);
            entity.Property(e => e.CompletedBy).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
            entity.Property(e => e.ActualCost).HasPrecision(18, 2);
            entity.Property(e => e.PartsCost).HasPrecision(18, 2);
            entity.Property(e => e.LaborCost).HasPrecision(18, 2);

            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.MaintenanceRecords)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Expense)
                .WithMany()
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Equipment Reservation
        builder.Entity<EquipmentReservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.ReservationNumber }).IsUnique();
            entity.Property(e => e.ReservationNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ConfirmedBy).HasMaxLength(200);
            entity.Property(e => e.CancelledBy).HasMaxLength(200);
            entity.Property(e => e.Purpose).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.Reservations)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Loan)
                .WithMany()
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Session)
                .WithMany()
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureProgramManagement(ModelBuilder builder)
    {
        // Program
        builder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.SubCategory).HasMaxLength(100);
            entity.Property(e => e.Sport).HasMaxLength(100);
            entity.Property(e => e.InstructorNames).HasMaxLength(500);
            entity.Property(e => e.QualificationName).HasMaxLength(200);
            entity.Property(e => e.QualificationBody).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.VideoUrl).HasMaxLength(500);
            entity.Property(e => e.PublishedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.EarlyBirdPrice).HasPrecision(18, 2);
            entity.Property(e => e.MemberPrice).HasPrecision(18, 2);
            entity.Property(e => e.NonMemberPrice).HasPrecision(18, 2);
            entity.Property(e => e.DepositAmount).HasPrecision(18, 2);
            entity.Property(e => e.SiblingDiscount).HasPrecision(18, 2);
            entity.Property(e => e.MultiProgramDiscount).HasPrecision(18, 2);
            entity.Property(e => e.CancellationFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalRevenue).HasPrecision(18, 2);
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PrimaryInstructor)
                .WithMany()
                .HasForeignKey(e => e.PrimaryInstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PrerequisiteProgram)
                .WithMany()
                .HasForeignKey(e => e.PrerequisiteProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Fee)
                .WithMany()
                .HasForeignKey(e => e.FeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Program Session
        builder.Entity<ProgramSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Topic).HasMaxLength(200);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.InstructorName).HasMaxLength(200);

            entity.HasOne(e => e.Program)
                .WithMany(p => p.Sessions)
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Instructor)
                .WithMany()
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Program Enrollment
        builder.Entity<ProgramEnrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.EnrollmentNumber }).IsUnique();
            entity.Property(e => e.EnrollmentNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.DiscountReason).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.WithdrawnBy).HasMaxLength(200);
            entity.Property(e => e.WithdrawalReason).HasMaxLength(500);
            entity.Property(e => e.CertificateNumber).HasMaxLength(100);
            entity.Property(e => e.CertificateUrl).HasMaxLength(500);
            entity.Property(e => e.EmergencyContactName).HasMaxLength(200);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(50);
            entity.Property(e => e.EmergencyContactRelation).HasMaxLength(100);
            entity.Property(e => e.GradeAwarded).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.BalanceDue).HasPrecision(18, 2);
            entity.Property(e => e.DepositAmount).HasPrecision(18, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);
            entity.Property(e => e.AttendanceRate).HasPrecision(5, 2);
            entity.Property(e => e.FinalScore).HasPrecision(5, 2);

            entity.HasOne(e => e.Program)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FamilyMember)
                .WithMany()
                .HasForeignKey(e => e.FamilyMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TransferredFrom)
                .WithMany()
                .HasForeignKey(e => e.TransferredFromId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Program Instructor
        builder.Entity<ProgramInstructor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProgramId, e.MemberId }).IsUnique();
            entity.Property(e => e.Role).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
            entity.Property(e => e.SessionRate).HasPrecision(18, 2);

            entity.HasOne(e => e.Program)
                .WithMany(p => p.Instructors)
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Program Attendance
        builder.Entity<ProgramAttendance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProgramSessionId, e.EnrollmentId }).IsUnique();
            entity.Property(e => e.ExcuseReason).HasMaxLength(500);
            entity.Property(e => e.CheckedInBy).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Score).HasPrecision(5, 2);

            entity.HasOne(e => e.ProgramSession)
                .WithMany(s => s.Attendance)
                .HasForeignKey(e => e.ProgramSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Enrollment)
                .WithMany(en => en.Attendance)
                .HasForeignKey(e => e.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MakeUpForSession)
                .WithMany()
                .HasForeignKey(e => e.MakeUpForSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Member Certificate
        builder.Entity<MemberCertificate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.CertificateNumber }).IsUnique().HasFilter("[CertificateNumber] IS NOT NULL");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CertificateNumber).HasMaxLength(100);
            entity.Property(e => e.AwardingBody).HasMaxLength(200);
            entity.Property(e => e.AwardedBy).HasMaxLength(200);
            entity.Property(e => e.Level).HasMaxLength(100);
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.VerificationUrl).HasMaxLength(500);
            entity.Property(e => e.VerificationCode).HasMaxLength(100);
            entity.Property(e => e.CertificateUrl).HasMaxLength(500);
            entity.Property(e => e.BadgeUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.RenewalFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Program)
                .WithMany()
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Enrollment)
                .WithMany()
                .HasForeignKey(e => e.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureEventManagementEnhanced(ModelBuilder builder)
    {
        // Event (enhanced)
        builder.Entity<Event>(entity =>
        {
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.SubCategory).HasMaxLength(100);
            entity.Property(e => e.Sport).HasMaxLength(100);
            entity.Property(e => e.TimeZone).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Room).HasMaxLength(100);
            entity.Property(e => e.MeetingPoint).HasMaxLength(200);
            entity.Property(e => e.OrganizerName).HasMaxLength(200);
            entity.Property(e => e.ContactName).HasMaxLength(200);
            entity.Property(e => e.ContactEmail).HasMaxLength(200);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.CheckInCode).HasMaxLength(50);
            entity.Property(e => e.CheckInQRCode).HasMaxLength(500);
            entity.Property(e => e.BannerUrl).HasMaxLength(500);
            entity.Property(e => e.VideoUrl).HasMaxLength(500);
            entity.Property(e => e.ExternalUrl).HasMaxLength(500);
            entity.Property(e => e.LiveStreamUrl).HasMaxLength(500);
            entity.Property(e => e.CancelledBy).HasMaxLength(200);
            entity.Property(e => e.PostponementReason).HasMaxLength(500);
            entity.Property(e => e.PublishedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.EarlyBirdPrice).HasPrecision(18, 2);
            entity.Property(e => e.GroupDiscount).HasPrecision(18, 2);
            entity.Property(e => e.CancellationFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalRevenue).HasPrecision(18, 2);
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Series)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.SeriesId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentEvent)
                .WithMany(e => e.ChildEvents)
                .HasForeignKey(e => e.ParentEventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Organizer)
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Fee)
                .WithMany()
                .HasForeignKey(e => e.FeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Event Series
        builder.Entity<EventSeries>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Event Session
        builder.Entity<EventSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Speaker).HasMaxLength(200);
            entity.Property(e => e.Room).HasMaxLength(100);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);

            entity.HasOne(e => e.Event)
                .WithMany(ev => ev.Sessions)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Event Ticket (enhanced)
        builder.Entity<EventTicket>(entity =>
        {
            entity.HasIndex(e => new { e.ClubId, e.TicketNumber }).IsUnique();
            entity.Property(e => e.TicketNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TicketType).HasMaxLength(100);
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.QRCode).HasMaxLength(500);
            entity.Property(e => e.AttendeeName).HasMaxLength(200);
            entity.Property(e => e.AttendeeEmail).HasMaxLength(200);
            entity.Property(e => e.AttendeePhone).HasMaxLength(50);
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.CheckedInBy).HasMaxLength(200);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Event RSVP (enhanced)
        builder.Entity<EventRSVP>(entity =>
        {
            entity.HasIndex(e => new { e.EventId, e.MemberId }).IsUnique().HasFilter("[MemberId] IS NOT NULL");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.CheckedInBy).HasMaxLength(200);
            entity.Property(e => e.PreviousResponse).HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(1000);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Event Registration
        builder.Entity<EventRegistration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.RegistrationNumber }).IsUnique();
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.DiscountReason).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.Property(e => e.CheckedInBy).HasMaxLength(200);
            entity.Property(e => e.CancelledBy).HasMaxLength(200);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.EmergencyContactName).HasMaxLength(200);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(50);
            entity.Property(e => e.EmergencyContactRelation).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.BalanceDue).HasPrecision(18, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Event)
                .WithMany(ev => ev.Registrations)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FamilyMember)
                .WithMany()
                .HasForeignKey(e => e.FamilyMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                .WithMany()
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureCompetitionManagement(ModelBuilder builder)
    {
        // Season
        builder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Competition
        builder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ClubId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Sport).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Division).HasMaxLength(100);
            entity.Property(e => e.Format).HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.OrganizerName).HasMaxLength(200);
            entity.Property(e => e.ContactEmail).HasMaxLength(200);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.BannerUrl).HasMaxLength(500);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.PublishedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.Property(e => e.EntryFee).HasPrecision(18, 2);
            entity.Property(e => e.TeamEntryFee).HasPrecision(18, 2);
            entity.Property(e => e.PlayerEntryFee).HasPrecision(18, 2);
            entity.Property(e => e.TotalPrizeMoney).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Season)
                .WithMany(s => s.Competitions)
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Organizer)
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Fee)
                .WithMany()
                .HasForeignKey(e => e.FeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Competition Round
        builder.Entity<CompetitionRound>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.Competition)
                .WithMany(c => c.Rounds)
                .HasForeignKey(e => e.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Competition Team
        builder.Entity<CompetitionTeam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.CompetitionId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Group).HasMaxLength(20);
            entity.Property(e => e.CaptainName).HasMaxLength(200);
            entity.Property(e => e.ContactEmail).HasMaxLength(200);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.HomeColors).HasMaxLength(100);
            entity.Property(e => e.AwayColors).HasMaxLength(100);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.Property(e => e.EntryFeeAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Competition)
                .WithMany(c => c.Teams)
                .HasForeignKey(e => e.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Club)
                .WithMany()
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Captain)
                .WithMany()
                .HasForeignKey(e => e.CaptainId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.HomeVenue)
                .WithMany()
                .HasForeignKey(e => e.HomeVenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Competition Participant
        builder.Entity<CompetitionParticipant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TeamId, e.MemberId }).IsUnique();
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Team)
                .WithMany(t => t.Participants)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Match
        builder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MatchNumber).HasMaxLength(50);
            entity.Property(e => e.RefereeName).HasMaxLength(200);
            entity.Property(e => e.FourthOfficial).HasMaxLength(200);
            entity.Property(e => e.Weather).HasMaxLength(100);
            entity.Property(e => e.PitchCondition).HasMaxLength(100);
            entity.Property(e => e.PostponementReason).HasMaxLength(500);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.HighlightsUrl).HasMaxLength(500);
            entity.Property(e => e.PhotosUrl).HasMaxLength(500);
            entity.Property(e => e.HomeConfirmedBy).HasMaxLength(200);
            entity.Property(e => e.AwayConfirmedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Competition)
                .WithMany(c => c.Matches)
                .HasForeignKey(e => e.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Round)
                .WithMany(r => r.Matches)
                .HasForeignKey(e => e.RoundId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Facility)
                .WithMany()
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(e => e.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(e => e.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Referee)
                .WithMany()
                .HasForeignKey(e => e.RefereeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FirstLegMatch)
                .WithMany()
                .HasForeignKey(e => e.FirstLegMatchId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Match Event
        builder.Entity<MatchEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);

            entity.HasOne(e => e.Match)
                .WithMany(m => m.Events)
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Participant)
                .WithMany(p => p.MatchEvents)
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AssistByParticipant)
                .WithMany()
                .HasForeignKey(e => e.AssistByParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SubstitutedForParticipant)
                .WithMany()
                .HasForeignKey(e => e.SubstitutedForParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Match Lineup
        builder.Entity<MatchLineup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.MatchId, e.ParticipantId }).IsUnique();
            entity.Property(e => e.Position).HasMaxLength(50);

            entity.HasOne(e => e.Match)
                .WithMany(m => m.Lineups)
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Participant)
                .WithMany()
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Competition Standing
        builder.Entity<CompetitionStanding>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.CompetitionId, e.TeamId }).IsUnique();
            entity.Property(e => e.Group).HasMaxLength(20);
            entity.Property(e => e.Form).HasMaxLength(20);
            entity.Property(e => e.Zone).HasMaxLength(50);

            entity.HasOne(e => e.Competition)
                .WithMany(c => c.Standings)
                .HasForeignKey(e => e.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
