namespace TheLeague.Core.Entities;

public enum TemplateType
{
    Email,
    Sms,
    PushNotification
}

public enum TemplateCategory
{
    Welcome,
    MembershipRenewal,
    PaymentReminder,
    PaymentReceipt,
    BookingConfirmation,
    BookingCancellation,
    EventReminder,
    EventInvitation,
    Birthday,
    Anniversary,
    PasswordReset,
    AccountVerification,
    GeneralAnnouncement,
    Custom
}

/// <summary>
/// Communication templates for automated and bulk messaging.
/// Supports variable substitution using {{variableName}} syntax.
/// </summary>
public class CommunicationTemplate
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TemplateType Type { get; set; } = TemplateType.Email;
    public TemplateCategory Category { get; set; } = TemplateCategory.Custom;

    // Email-specific
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;
    public string? HtmlBody { get; set; } // Rich HTML version

    // Available variables for this template (JSON array)
    // e.g., ["memberName", "clubName", "renewalDate", "amount"]
    public string? AvailableVariables { get; set; }

    public bool IsDefault { get; set; } // System default template
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
}
