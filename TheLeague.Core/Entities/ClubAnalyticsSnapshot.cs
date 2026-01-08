namespace TheLeague.Core.Entities;

public class ClubAnalyticsSnapshot
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public DateTime SnapshotDate { get; set; }

    // Membership Stats
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int NewMembersThisMonth { get; set; }
    public int ExpiredMemberships { get; set; }

    // Financial Stats
    public decimal TotalRevenueThisMonth { get; set; }
    public decimal TotalRevenueThisYear { get; set; }
    public decimal OutstandingPayments { get; set; }

    // Engagement Stats
    public int SessionsThisMonth { get; set; }
    public int TotalBookingsThisMonth { get; set; }
    public decimal AverageAttendanceRate { get; set; }
    public int EventsThisMonth { get; set; }

    public Club Club { get; set; } = null!;
}
