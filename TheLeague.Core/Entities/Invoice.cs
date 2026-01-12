using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents an invoice sent to a member.
/// Contains line items and tracks payment status.
/// </summary>
public class Invoice
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    // Invoice Number & Reference
    public string InvoiceNumber { get; set; } = string.Empty;
    public string? PurchaseOrderNumber { get; set; }
    public string? ExternalReference { get; set; }

    // Dates
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ViewedDate { get; set; }
    public DateTime? VoidedDate { get; set; }

    // Amounts
    public decimal SubTotal { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountDescription { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceDue { get; set; }
    public string Currency { get; set; } = "GBP";

    // Tax Details
    public string? TaxNumber { get; set; } // VAT number if applicable
    public decimal? TaxRate { get; set; }
    public bool IsTaxExempt { get; set; }
    public string? TaxExemptReason { get; set; }

    // Status
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public CollectionStatus CollectionStatus { get; set; } = CollectionStatus.Current;

    // Billing Address (snapshot at time of invoice)
    public string? BillingName { get; set; }
    public string? BillingAddress { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingPostcode { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingEmail { get; set; }

    // Payment Terms
    public int PaymentTermsDays { get; set; } = 30;
    public string? PaymentInstructions { get; set; }
    public bool AllowPartialPayment { get; set; } = true;
    public bool AllowOnlinePayment { get; set; } = true;

    // Late Payment
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }
    public decimal? LateFeeApplied { get; set; }
    public DateTime? LateFeeAppliedDate { get; set; }

    // Communication
    public int RemindersSent { get; set; }
    public DateTime? LastReminderDate { get; set; }
    public DateTime? FinalNoticeDate { get; set; }

    // Notes
    public string? Notes { get; set; } // Shown on invoice
    public string? InternalNotes { get; set; } // Staff only
    public string? TermsAndConditions { get; set; }

    // Document
    public string? PdfUrl { get; set; }
    public string? PublicViewUrl { get; set; } // Shareable link

    // Corporate/Family Billing
    public bool IsCorporateInvoice { get; set; }
    public string? CorporateName { get; set; }
    public Guid? PrimaryMemberId { get; set; } // For family billing

    // Voiding
    public string? VoidReason { get; set; }
    public string? VoidedBy { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Member? PrimaryMember { get; set; }
    public ICollection<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

/// <summary>
/// Represents a line item on an invoice.
/// </summary>
public class InvoiceLineItem
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid? FeeId { get; set; }

    // Item Details
    public string Description { get; set; } = string.Empty;
    public FeeType? FeeType { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TaxRate { get; set; }
    public decimal Total { get; set; }

    // Reference
    public string? ServicePeriod { get; set; } // e.g., "Jan 2025 - Dec 2025"
    public DateTime? ServiceStartDate { get; set; }
    public DateTime? ServiceEndDate { get; set; }
    public Guid? RelatedEntityId { get; set; } // Membership, Event, etc.
    public string? RelatedEntityType { get; set; }

    // GL/Accounting
    public string? GLAccountCode { get; set; }
    public string? CostCenter { get; set; }
    public string? TaxCode { get; set; }

    public int SortOrder { get; set; }

    // Navigation
    public Invoice Invoice { get; set; } = null!;
    public Fee? Fee { get; set; }
}
