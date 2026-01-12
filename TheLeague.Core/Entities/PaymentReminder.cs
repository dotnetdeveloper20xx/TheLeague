using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Tracks payment reminders sent to members.
/// Supports automated reminder schedules and escalation.
/// </summary>
public class PaymentReminder
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    // Related Entities
    public Guid? InvoiceId { get; set; }
    public Guid? PaymentPlanId { get; set; }
    public Guid? InstallmentId { get; set; }
    public Guid? MembershipId { get; set; }

    // Reminder Details
    public ReminderType Type { get; set; }
    public int ReminderSequence { get; set; } // 1st, 2nd, 3rd reminder
    public decimal AmountDue { get; set; }
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
    public string Currency { get; set; } = "GBP";

    // Communication
    public string? Channel { get; set; } // Email, SMS, Push
    public string? ToEmail { get; set; }
    public string? ToPhone { get; set; }
    public string? Subject { get; set; }
    public string? MessageBody { get; set; }
    public Guid? TemplateId { get; set; }

    // Status
    public bool IsSent { get; set; }
    public DateTime? SentDate { get; set; }
    public bool IsDelivered { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public bool IsOpened { get; set; }
    public DateTime? OpenedDate { get; set; }
    public bool HasBounced { get; set; }
    public string? BounceReason { get; set; }

    // Scheduling
    public DateTime ScheduledDate { get; set; }
    public bool IsScheduled { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancellationReason { get; set; }

    // Escalation
    public bool IsEscalation { get; set; }
    public int EscalationLevel { get; set; } // 0=initial, 1=first escalation, etc.
    public string? EscalationAction { get; set; } // AccessRestriction, CollectionReferral, etc.
    public DateTime? EscalationDate { get; set; }

    // Response Tracking
    public bool HasResponse { get; set; }
    public DateTime? ResponseDate { get; set; }
    public string? ResponseNotes { get; set; }
    public bool PaymentReceived { get; set; }
    public Guid? PaymentId { get; set; }

    // Retry
    public int RetryCount { get; set; }
    public DateTime? LastRetryDate { get; set; }
    public string? RetryReason { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Invoice? Invoice { get; set; }
    public PaymentPlan? PaymentPlan { get; set; }
    public PaymentInstallment? Installment { get; set; }
    public Membership? Membership { get; set; }
    public Payment? Payment { get; set; }
    public CommunicationTemplate? Template { get; set; }
}
