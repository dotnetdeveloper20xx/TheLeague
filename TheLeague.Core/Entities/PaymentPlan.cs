using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a payment plan that allows members to pay in installments.
/// Tracks the overall plan status and links to individual installments.
/// </summary>
public class PaymentPlan
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? MembershipId { get; set; }
    public Guid? InvoiceId { get; set; }

    // Plan Details
    public string PlanName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PlanReference { get; set; }

    // Amounts
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal? SetupFee { get; set; }
    public decimal? InterestRate { get; set; } // Annual interest rate %
    public decimal? TotalInterest { get; set; }
    public string Currency { get; set; } = "GBP";

    // Installment Configuration
    public int TotalInstallments { get; set; }
    public int PaidInstallments { get; set; }
    public int RemainingInstallments { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal? FinalInstallmentAmount { get; set; } // May differ due to rounding
    public FeeFrequency Frequency { get; set; } = FeeFrequency.Monthly;
    public int? FrequencyInterval { get; set; } = 1; // Every N periods
    public int? PaymentDayOfMonth { get; set; }

    // Dates
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }

    // Status
    public PaymentPlanStatus Status { get; set; } = PaymentPlanStatus.Active;
    public bool IsAutoDebit { get; set; }
    public string? DefaultPaymentMethod { get; set; } // Stored payment method ID

    // Late Payment Handling
    public int MissedPayments { get; set; }
    public int MaxMissedPayments { get; set; } = 2; // Before default
    public decimal? LateFeePerMissed { get; set; }
    public decimal TotalLateFees { get; set; }

    // Agreement
    public bool AgreementSigned { get; set; }
    public DateTime? AgreementSignedDate { get; set; }
    public string? AgreementDocumentUrl { get; set; }
    public string? TermsAndConditions { get; set; }

    // Cancellation
    public DateTime? CancelledDate { get; set; }
    public string? CancellationReason { get; set; }
    public string? CancelledBy { get; set; }
    public decimal? EarlyCancellationFee { get; set; }

    // Pause
    public bool IsPaused { get; set; }
    public DateTime? PausedDate { get; set; }
    public DateTime? ResumeDate { get; set; }
    public string? PauseReason { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Membership? Membership { get; set; }
    public Invoice? Invoice { get; set; }
    public ICollection<PaymentInstallment> Installments { get; set; } = new List<PaymentInstallment>();
}

/// <summary>
/// Represents a single installment within a payment plan.
/// </summary>
public class PaymentInstallment
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid PaymentPlanId { get; set; }
    public Guid MemberId { get; set; }

    // Installment Details
    public int InstallmentNumber { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? InterestAmount { get; set; }
    public decimal? LateFee { get; set; }
    public decimal TotalDue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public string Currency { get; set; } = "GBP";

    // Dates
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? ScheduledDate { get; set; } // For auto-debit

    // Status
    public InstallmentStatus Status { get; set; } = InstallmentStatus.Pending;
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }

    // Payment Information
    public Guid? PaymentId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionReference { get; set; }

    // Auto-Debit
    public bool IsAutoDebit { get; set; }
    public int RetryCount { get; set; }
    public DateTime? LastRetryDate { get; set; }
    public string? FailureReason { get; set; }

    // Reminders
    public bool ReminderSent { get; set; }
    public DateTime? ReminderSentDate { get; set; }
    public bool OverdueNoticeSent { get; set; }
    public DateTime? OverdueNoticeSentDate { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public PaymentPlan PaymentPlan { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Payment? Payment { get; set; }
}
