using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class MemberNote
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    public MemberNoteType Type { get; set; } = MemberNoteType.General;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPrivate { get; set; } = true; // Only visible to staff
    public bool IsPinned { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
}
