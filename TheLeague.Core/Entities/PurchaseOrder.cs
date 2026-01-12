using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a purchase order for goods or services.
/// Tracks order details, approval workflow, and receiving status.
/// </summary>
public class PurchaseOrder
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid VendorId { get; set; }

    // PO Details
    public string PONumber { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }

    // Amounts
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "GBP";

    // Status
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    public DateTime? SentDate { get; set; }
    public string? SentTo { get; set; }
    public string? SentVia { get; set; } // Email, Fax, Post

    // Receiving
    public bool IsFullyReceived { get; set; }
    public bool IsPartiallyReceived { get; set; }
    public DateTime? FirstReceivedDate { get; set; }
    public DateTime? LastReceivedDate { get; set; }
    public decimal ReceivedAmount { get; set; }
    public decimal OutstandingAmount { get; set; }

    // Billing
    public bool IsFullyBilled { get; set; }
    public bool IsPartiallyBilled { get; set; }
    public decimal BilledAmount { get; set; }
    public decimal UnbilledAmount { get; set; }

    // Shipping
    public string? ShipToName { get; set; }
    public string? ShipToAddress { get; set; }
    public string? ShipToCity { get; set; }
    public string? ShipToPostCode { get; set; }
    public string? ShippingMethod { get; set; }
    public string? TrackingNumber { get; set; }

    // Payment Terms
    public PaymentTerms PaymentTerms { get; set; } = PaymentTerms.Net30;
    public int? CustomPaymentDays { get; set; }

    // Categorization
    public string? Department { get; set; }
    public string? Project { get; set; }
    public string? CostCenter { get; set; }
    public Guid? FiscalPeriodId { get; set; }

    // Approval
    public bool RequiresApproval { get; set; }
    public int ApprovalLevel { get; set; }
    public string? CurrentApprover { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? RejectedBy { get; set; }
    public DateTime? RejectedDate { get; set; }
    public string? RejectionReason { get; set; }

    // Vendor Acknowledgment
    public bool VendorAcknowledged { get; set; }
    public DateTime? VendorAcknowledgedDate { get; set; }
    public string? VendorReference { get; set; }

    // Close/Cancel
    public DateTime? ClosedDate { get; set; }
    public string? ClosedBy { get; set; }
    public string? CloseReason { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancelledBy { get; set; }
    public string? CancellationReason { get; set; }

    // Terms & Conditions
    public string? TermsAndConditions { get; set; }
    public string? SpecialInstructions { get; set; }

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
    public Vendor Vendor { get; set; } = null!;
    public FiscalPeriod? FiscalPeriod { get; set; }
    public ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<PurchaseOrderReceipt> Receipts { get; set; } = new List<PurchaseOrderReceipt>();
}

/// <summary>
/// Represents a line item on a purchase order.
/// </summary>
public class PurchaseOrderLine
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid? AccountId { get; set; }

    // Line Details
    public int LineNumber { get; set; }
    public string? ItemCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? UnitOfMeasure { get; set; }

    // Quantities
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityOutstanding { get; set; }
    public decimal QuantityBilled { get; set; }

    // Pricing
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";

    // Tax
    public Guid? TaxRateId { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }

    // Categorization
    public ExpenseCategory? Category { get; set; }
    public string? Department { get; set; }
    public string? Project { get; set; }
    public string? CostCenter { get; set; }

    // Delivery
    public DateTime? RequiredDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public bool IsFullyReceived { get; set; }
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Navigation
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public ChartOfAccount? Account { get; set; }
    public TaxRate? TaxRate { get; set; }
    public ICollection<ExpenseLineItem> ExpenseLineItems { get; set; } = new List<ExpenseLineItem>();
}

/// <summary>
/// Represents a goods receipt against a purchase order.
/// </summary>
public class PurchaseOrderReceipt
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }

    // Receipt Details
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public string? ReceivedBy { get; set; }
    public string? DeliveryNote { get; set; }
    public string? CarrierName { get; set; }
    public string? TrackingNumber { get; set; }

    // Quality
    public bool QualityChecked { get; set; }
    public DateTime? QualityCheckDate { get; set; }
    public string? QualityCheckedBy { get; set; }
    public bool PassedQualityCheck { get; set; }
    public string? QualityNotes { get; set; }

    // Issues
    public bool HasDamage { get; set; }
    public string? DamageDescription { get; set; }
    public bool HasShortage { get; set; }
    public string? ShortageDescription { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public ICollection<PurchaseOrderReceiptLine> Lines { get; set; } = new List<PurchaseOrderReceiptLine>();
}

/// <summary>
/// Represents a line item on a goods receipt.
/// </summary>
public class PurchaseOrderReceiptLine
{
    public Guid Id { get; set; }
    public Guid ReceiptId { get; set; }
    public Guid PurchaseOrderLineId { get; set; }

    // Quantities
    public decimal QuantityReceived { get; set; }
    public decimal QuantityAccepted { get; set; }
    public decimal QuantityRejected { get; set; }

    // Quality
    public string? RejectionReason { get; set; }
    public string? Condition { get; set; } // Good, Damaged, etc.

    // Storage
    public string? StorageLocation { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Navigation
    public PurchaseOrderReceipt Receipt { get; set; } = null!;
    public PurchaseOrderLine PurchaseOrderLine { get; set; } = null!;
}
