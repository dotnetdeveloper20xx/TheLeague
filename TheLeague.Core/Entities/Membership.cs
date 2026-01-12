using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a member's subscription to a membership type.
/// Tracks the full lifecycle including renewals, freezes, and cancellations.
/// </summary>
public class Membership
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid MembershipTypeId { get; set; }

    // Term Details
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? OriginalStartDate { get; set; } // First time member joined this type
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Annual;
    public MembershipStatus Status { get; set; }

    // Pricing
    public decimal BasePrice { get; set; } // Original price at time of signup
    public decimal CurrentPrice { get; set; } // Current price (may be grandfathered)
    public decimal DiscountAmount { get; set; } // Total discount applied
    public string? DiscountCode { get; set; } // Promo code used
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public bool JoiningFeePaid { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment Tracking
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public int ConsecutivePaymentsOnTime { get; set; }
    public int MissedPaymentCount { get; set; }
    public DateTime? LastMissedPaymentDate { get; set; }

    // Renewal
    public bool AutoRenew { get; set; }
    public int RenewalCount { get; set; } // Number of times renewed
    public DateTime? LastRenewalDate { get; set; }
    public bool RenewalReminderSent { get; set; }
    public DateTime? RenewalReminderSentDate { get; set; }

    // Grace Period
    public bool InGracePeriod { get; set; }
    public DateTime? GracePeriodEndDate { get; set; }
    public bool GracePeriodNoticeSent { get; set; }

    // Freeze/Pause
    public bool IsFrozen { get; set; }
    public DateTime? FreezeStartDate { get; set; }
    public DateTime? FreezeEndDate { get; set; }
    public FreezeReason? FreezeReason { get; set; }
    public string? FreezeNotes { get; set; }
    public int TotalFreezeDaysUsed { get; set; } // Total freeze days used this year
    public DateTime? FreezeYearResetDate { get; set; }

    // Cancellation
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }
    public DateTime? CancellationEffectiveDate { get; set; } // When access ends
    public DateTime? CancellationRequestDate { get; set; }
    public CancellationReason? CancellationReason { get; set; }
    public string? CancellationFeedback { get; set; } // Exit survey responses
    public string? CancelledBy { get; set; } // User who cancelled
    public decimal? CancellationFeeCharged { get; set; }
    public bool EligibleForReinstatement { get; set; } = true;

    // Upgrade/Downgrade History
    public Guid? PreviousMembershipTypeId { get; set; } // For tracking history
    public DateTime? UpgradeDowngradeDate { get; set; }
    public string? ChangeReason { get; set; }

    // Access Control
    public bool AccessSuspended { get; set; }
    public string? SuspensionReason { get; set; }
    public DateTime? SuspendedUntil { get; set; }
    public int GuestPassesUsed { get; set; }
    public DateTime? GuestPassResetDate { get; set; }

    // Metadata
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; } // Staff-only notes
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public MembershipType MembershipType { get; set; } = null!;
    public MembershipType? PreviousMembershipType { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<MembershipFreeze> FreezeHistory { get; set; } = new List<MembershipFreeze>();
}
