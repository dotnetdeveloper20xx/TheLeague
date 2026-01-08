namespace TheLeague.Core.Entities;

public class RecurringBooking
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }
    public Guid RecurringScheduleId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public RecurringSchedule RecurringSchedule { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public FamilyMember? FamilyMember { get; set; }
}
