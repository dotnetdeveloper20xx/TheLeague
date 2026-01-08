using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class RecurringSchedule
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SessionCategory Category { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Capacity { get; set; }

    public DateTime ScheduleStartDate { get; set; }
    public DateTime? ScheduleEndDate { get; set; }

    public bool IsActive { get; set; } = true;
    public decimal? SessionFee { get; set; }

    public Venue? Venue { get; set; }
    public ICollection<Session> GeneratedSessions { get; set; } = new List<Session>();
    public ICollection<RecurringBooking> RecurringBookings { get; set; } = new List<RecurringBooking>();
}
