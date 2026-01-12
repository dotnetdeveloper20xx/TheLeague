using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a financial transaction/payment in the system.
/// Supports multiple payment methods, partial payments, and refunds.
/// </summary>
public class Payment
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    // Related Entities
    public Guid? MembershipId { get; set; }
    public Guid? EventTicketId { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? PaymentPlanId { get; set; }
    public Guid? InstallmentId { get; set; }
    public Guid? FeeId { get; set; }

    // Amount Details
    public decimal Amount { get; set; }
    public decimal? OriginalAmount { get; set; } // Before discounts
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TipAmount { get; set; }
    public decimal? ProcessingFee { get; set; } // Card processing fees
    public decimal NetAmount { get; set; } // Amount after fees
    public string Currency { get; set; } = "GBP";

    // Payment Information
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentType Type { get; set; }
    public FeeType? FeeType { get; set; }
    public TransactionType TransactionType { get; set; } = TransactionType.Payment;

    // Description & Reference
    public string? Description { get; set; }
    public string? InternalNotes { get; set; }
    public string? ReferenceNumber { get; set; } // Internal reference
    public string? ExternalReference { get; set; } // Customer reference

    // Dates
    public DateTime PaymentDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? SettlementDate { get; set; } // When funds were settled

    // External Payment Provider References
    public string? StripePaymentIntentId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? PayPalTransactionId { get; set; }
    public string? PayPalOrderId { get; set; }
    public string? GoCardlessPaymentId { get; set; }
    public string? BankTransferReference { get; set; }

    // Receipt Information
    public string? ReceiptNumber { get; set; }
    public string? ReceiptUrl { get; set; }
    public bool ReceiptSent { get; set; }
    public DateTime? ReceiptSentDate { get; set; }

    // Card Details (last 4, for reference)
    public string? CardLast4 { get; set; }
    public string? CardBrand { get; set; } // Visa, Mastercard, etc.
    public string? CardExpiryMonth { get; set; }
    public string? CardExpiryYear { get; set; }

    // Bank Transfer Details
    public string? BankAccountLast4 { get; set; }
    public string? BankName { get; set; }

    // Manual Payment
    public string? ManualPaymentReference { get; set; }
    public string? RecordedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    // Recurring/Subscription
    public bool IsRecurring { get; set; }
    public Guid? RecurringPaymentId { get; set; } // Parent recurring payment setup
    public int? RecurringSequence { get; set; } // Sequence number in recurring series

    // Partial Payment
    public bool IsPartialPayment { get; set; }
    public decimal? TotalAmountOwed { get; set; }
    public decimal? RemainingBalance { get; set; }
    public Guid? ParentPaymentId { get; set; } // For split payments

    // Refund Tracking
    public bool IsRefunded { get; set; }
    public bool IsPartiallyRefunded { get; set; }
    public decimal? RefundedAmount { get; set; }
    public Guid? RefundId { get; set; }

    // Failure Handling
    public int RetryCount { get; set; }
    public DateTime? LastRetryDate { get; set; }
    public string? FailureReason { get; set; }
    public string? FailureCode { get; set; }

    // Metadata
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public Membership? Membership { get; set; }
    public Invoice? Invoice { get; set; }
    public PaymentPlan? PaymentPlan { get; set; }
    public PaymentInstallment? Installment { get; set; }
    public Fee? Fee { get; set; }
    public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
