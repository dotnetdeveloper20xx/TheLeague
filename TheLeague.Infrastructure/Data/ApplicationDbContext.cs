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
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
    public DbSet<BulkEmailCampaign> BulkEmailCampaigns => Set<BulkEmailCampaign>();
    public DbSet<ClubAnalyticsSnapshot> ClubAnalyticsSnapshots => Set<ClubAnalyticsSnapshot>();

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
            builder.Entity<EmailLog>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<BulkEmailCampaign>().HasQueryFilter(e => e.ClubId == currentTenantId);
            builder.Entity<ClubAnalyticsSnapshot>().HasQueryFilter(e => e.ClubId == currentTenantId);
        }

        // Configure entity relationships
        ConfigureClub(builder);
        ConfigureMember(builder);
        ConfigureMembership(builder);
        ConfigureSession(builder);
        ConfigureEvent(builder);
        ConfigureVenue(builder);
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
            entity.Property(e => e.AnnualFee).HasPrecision(18, 2);
            entity.Property(e => e.MonthlyFee).HasPrecision(18, 2);
            entity.Property(e => e.SessionFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.MembershipTypes)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.AmountDue).HasPrecision(18, 2);

            entity.HasOne(e => e.Member)
                .WithMany(e => e.Memberships)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MembershipType)
                .WithMany(e => e.Memberships)
                .HasForeignKey(e => e.MembershipTypeId)
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
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.TicketCode).HasMaxLength(50);
            entity.HasIndex(e => e.TicketCode).IsUnique();

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(e => e.EventTickets)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<EventRSVP>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.MemberId }).IsUnique();

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
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);

            entity.HasOne(e => e.Club)
                .WithMany(e => e.Venues)
                .HasForeignKey(e => e.ClubId)
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
    }
}
