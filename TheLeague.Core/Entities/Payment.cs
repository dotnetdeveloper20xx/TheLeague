using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? MembershipId { get; set; }
    public Guid? EventTicketId { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentType Type { get; set; }

    public string? Description { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime? ProcessedDate { get; set; }

    // External References
    public string? StripePaymentIntentId { get; set; }
    public string? PayPalTransactionId { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? ReceiptUrl { get; set; }

    // For manual payments
    public string? ManualPaymentReference { get; set; }
    public string? RecordedBy { get; set; }

    public Member Member { get; set; } = null!;
    public Membership? Membership { get; set; }
}
