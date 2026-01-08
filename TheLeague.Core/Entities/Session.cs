using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Session
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SessionCategory Category { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int CurrentBookings { get; set; }

    public bool IsRecurring { get; set; }
    public Guid? RecurringScheduleId { get; set; }

    public decimal? SessionFee { get; set; }
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }

    public Club Club { get; set; } = null!;
    public Venue? Venue { get; set; }
    public RecurringSchedule? RecurringSchedule { get; set; }
    public ICollection<SessionBooking> Bookings { get; set; } = new List<SessionBooking>();
    public ICollection<Waitlist> WaitlistEntries { get; set; } = new List<Waitlist>();
}
