namespace TheLeague.Core.Entities;

public class Waitlist
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid SessionId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }

    public int Position { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool NotificationSent { get; set; }
    public DateTime? NotificationSentAt { get; set; }

    public Session Session { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
