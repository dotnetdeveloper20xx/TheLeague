using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Tracks a member's account balance/wallet.
/// Supports prepaid credits, outstanding dues, and transaction history.
/// </summary>
public class MemberBalance
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    // Current Balance
    public decimal CreditBalance { get; set; } // Prepaid/wallet balance
    public decimal OutstandingBalance { get; set; } // Amount owed
    public decimal NetBalance { get; set; } // Credit - Outstanding
    public string Currency { get; set; } = "GBP";

    // Collection Status
    public CollectionStatus CollectionStatus { get; set; } = CollectionStatus.Current;
    public DateTime? LastStatusChangeDate { get; set; }

    // Outstanding Details
    public decimal Current { get; set; } // Not yet due
    public decimal Overdue30 { get; set; }
    public decimal Overdue60 { get; set; }
    public decimal Overdue90 { get; set; }
    public decimal Overdue120Plus { get; set; }
    public DateTime? OldestDueDate { get; set; }
    public int TotalOverdueItems { get; set; }

    // Credit Details
    public DateTime? LastCreditDate { get; set; }
    public DateTime? CreditExpiryDate { get; set; }
    public decimal? ExpiringCredit { get; set; } // Credits expiring soon
    public DateTime? ExpiringCreditDate { get; set; }

    // Payment History Summary
    public decimal TotalPaidAllTime { get; set; }
    public decimal TotalPaidThisYear { get; set; }
    public decimal TotalChargedAllTime { get; set; }
    public decimal TotalChargedThisYear { get; set; }
    public decimal TotalRefundedAllTime { get; set; }
    public decimal TotalWrittenOff { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal? LastPaymentAmount { get; set; }

    // Payment Behavior
    public int ConsecutiveOnTimePayments { get; set; }
    public int TotalLatePayments { get; set; }
    public int TotalMissedPayments { get; set; }
    public decimal AveragePaymentDelay { get; set; } // Days
    public string? PaymentRating { get; set; } // Good, Fair, Poor

    // Auto-Pay Setup
    public bool HasAutoPaySetup { get; set; }
    public string? DefaultPaymentMethodId { get; set; }
    public string? DefaultPaymentMethodType { get; set; }
    public string? DefaultPaymentMethodLast4 { get; set; }

    // Limits & Restrictions
    public decimal? CreditLimit { get; set; }
    public bool IsAccessRestricted { get; set; } // Due to outstanding balance
    public DateTime? AccessRestrictedDate { get; set; }
    public string? AccessRestrictionReason { get; set; }

    // Payment Arrangement
    public bool HasPaymentArrangement { get; set; }
    public Guid? PaymentPlanId { get; set; }
    public string? ArrangementNotes { get; set; }

    // Communication
    public DateTime? LastReminderSent { get; set; }
    public int RemindersSentThisMonth { get; set; }
    public bool DoNotContact { get; set; } // Pause reminders
    public DateTime? DoNotContactUntil { get; set; }

    // Metadata
    public DateTime LastCalculatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public PaymentPlan? PaymentPlan { get; set; }
    public ICollection<BalanceTransaction> Transactions { get; set; } = new List<BalanceTransaction>();
}

/// <summary>
/// Represents a transaction that affects a member's balance.
/// </summary>
public class BalanceTransaction
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberBalanceId { get; set; }
    public Guid MemberId { get; set; }

    // Transaction Details
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Currency { get; set; } = "GBP";

    // Description
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public FeeType? FeeType { get; set; }

    // Related Entities
    public Guid? PaymentId { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? RefundId { get; set; }
    public Guid? MembershipId { get; set; }

    // Dates
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; } // For credits

    // Source
    public string? Source { get; set; } // System, Manual, Import, etc.
    public string? ProcessedBy { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public MemberBalance MemberBalance { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Payment? Payment { get; set; }
    public Invoice? Invoice { get; set; }
    public Refund? Refund { get; set; }
}
