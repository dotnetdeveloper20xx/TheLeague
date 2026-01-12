using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a refund issued for a payment.
/// Tracks refund status, reason, and processing details.
/// </summary>
public class Refund
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid PaymentId { get; set; }
    public Guid MemberId { get; set; }

    // Refund Details
    public string RefundNumber { get; set; } = string.Empty;
    public decimal OriginalPaymentAmount { get; set; }
    public decimal RefundAmount { get; set; }
    public bool IsFullRefund { get; set; }
    public string Currency { get; set; } = "GBP";

    // Status
    public RefundStatus Status { get; set; } = RefundStatus.Requested;
    public RefundReason Reason { get; set; }
    public string? ReasonDetails { get; set; }

    // Dates
    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? RejectedDate { get; set; }

    // Approval Workflow
    public bool RequiresApproval { get; set; }
    public string? RequestedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ProcessedBy { get; set; }
    public string? RejectedBy { get; set; }
    public string? RejectionReason { get; set; }

    // Refund Method
    public PaymentMethod RefundMethod { get; set; }
    public bool RefundToOriginalMethod { get; set; } = true;
    public string? RefundToAccount { get; set; } // If different from original

    // External References
    public string? StripeRefundId { get; set; }
    public string? PayPalRefundId { get; set; }
    public string? BankTransferReference { get; set; }
    public string? ChequeNumber { get; set; }

    // Credit Option
    public bool RefundAsCredit { get; set; } // Refund to member balance
    public Guid? BalanceTransactionId { get; set; }

    // Documentation
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? SupportingDocuments { get; set; } // JSON array of URLs

    // Impact
    public Guid? AffectedMembershipId { get; set; }
    public Guid? AffectedEventTicketId { get; set; }
    public bool MembershipCancelled { get; set; }
    public bool EventTicketCancelled { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public BalanceTransaction? BalanceTransaction { get; set; }
}
