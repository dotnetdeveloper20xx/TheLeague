using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class SessionBooking
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid SessionId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public bool Attended { get; set; }
    public DateTime? CheckedInAt { get; set; }

    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }

    public Session Session { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public FamilyMember? FamilyMember { get; set; }
}
