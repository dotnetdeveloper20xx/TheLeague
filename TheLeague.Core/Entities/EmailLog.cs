using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class EmailLog
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }

    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public EmailType Type { get; set; }

    public EmailStatus Status { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string? SendGridMessageId { get; set; }
    public string? ErrorMessage { get; set; }

    public Member? Member { get; set; }
}
