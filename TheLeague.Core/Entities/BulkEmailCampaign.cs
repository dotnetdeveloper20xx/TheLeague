using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class BulkEmailCampaign
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? RecipientFilter { get; set; }
    public int TotalRecipients { get; set; }
    public int SentCount { get; set; }
    public int FailedCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public string? CreatedBy { get; set; }

    public CampaignStatus Status { get; set; }
}
