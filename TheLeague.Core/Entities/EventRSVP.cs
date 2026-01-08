using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class EventRSVP
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid EventId { get; set; }
    public Guid MemberId { get; set; }

    public RSVPResponse Response { get; set; }
    public int GuestCount { get; set; }
    public string? Notes { get; set; }
    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;

    public Event Event { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
