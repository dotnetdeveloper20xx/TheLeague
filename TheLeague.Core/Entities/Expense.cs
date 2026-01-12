using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents an expense or bill to be paid.
/// Tracks expense details, approval workflow, and payment status.
/// </summary>
public class Expense
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VendorId { get; set; }
    public Guid? PurchaseOrderId { get; set; }

    // Expense Details
    public string ExpenseNumber { get; set; } = string.Empty;
    public string? Reference { get; set; } // Invoice number from vendor
    public string Description { get; set; } = string.Empty;
    public ExpenseCategory Category { get; set; }
    public string? SubCategory { get; set; }
    public DateTime ExpenseDate { get; set; }
    public DateTime? ReceivedDate { get; set; }

    // Amounts
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceDue { get; set; }
    public string Currency { get; set; } = "GBP";
    public decimal? ExchangeRate { get; set; }
    public string? OriginalCurrency { get; set; }
    public decimal? OriginalAmount { get; set; }

    // Payment Terms
    public DateTime DueDate { get; set; }
    public PaymentTerms PaymentTerms { get; set; } = PaymentTerms.Net30;
    public int DaysUntilDue { get; set; }
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }

    // Status
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Draft;
    public bool IsPaid { get; set; }
    public bool IsPartiallyPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaidBy { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }

    // Categorization
    public string? Department { get; set; }
    public string? Project { get; set; }
    public string? CostCenter { get; set; }
    public Guid? FiscalPeriodId { get; set; }

    // Accounting
    public Guid? DefaultAccountId { get; set; }
    public Guid? JournalEntryId { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? PostedBy { get; set; }

    // Approval
    public bool RequiresApproval { get; set; }
    public int ApprovalLevel { get; set; }
    public decimal ApprovalThreshold { get; set; }
    public string? CurrentApprover { get; set; }
    public DateTime? ApprovalDueDate { get; set; }

    // Recurring
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public DateTime? NextRecurrenceDate { get; set; }
    public Guid? ParentExpenseId { get; set; }

    // Vendor Bill Details
    public string? VendorBillNumber { get; set; }
    public DateTime? VendorBillDate { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Void
    public bool IsVoided { get; set; }
    public DateTime? VoidedDate { get; set; }
    public string? VoidedBy { get; set; }
    public string? VoidReason { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? SubmittedBy { get; set; }
    public DateTime? SubmittedDate { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Vendor? Vendor { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public ChartOfAccount? DefaultAccount { get; set; }
    public FiscalPeriod? FiscalPeriod { get; set; }
    public JournalEntry? JournalEntry { get; set; }
    public Expense? ParentExpense { get; set; }
    public ICollection<ExpenseLineItem> LineItems { get; set; } = new List<ExpenseLineItem>();
    public ICollection<ExpenseApproval> Approvals { get; set; } = new List<ExpenseApproval>();
    public ICollection<ExpenseAttachment> Attachments { get; set; } = new List<ExpenseAttachment>();
    public ICollection<ExpensePayment> Payments { get; set; } = new List<ExpensePayment>();
}

/// <summary>
/// Represents a line item on an expense.
/// </summary>
public class ExpenseLineItem
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Guid AccountId { get; set; }

    // Line Details
    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";

    // Tax
    public Guid? TaxRateId { get; set; }
    public decimal TaxAmount { get; set; }
    public bool IsTaxInclusive { get; set; }
    public decimal TotalAmount { get; set; }

    // Categorization
    public ExpenseCategory? Category { get; set; }
    public string? Department { get; set; }
    public string? Project { get; set; }
    public string? CostCenter { get; set; }

    // Related
    public Guid? PurchaseOrderLineId { get; set; }

    // Billable
    public bool IsBillable { get; set; }
    public Guid? BillToMemberId { get; set; }
    public bool IsBilled { get; set; }
    public Guid? BilledInvoiceId { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Navigation
    public Expense Expense { get; set; } = null!;
    public ChartOfAccount Account { get; set; } = null!;
    public TaxRate? TaxRate { get; set; }
    public PurchaseOrderLine? PurchaseOrderLine { get; set; }
    public Member? BillToMember { get; set; }
}

/// <summary>
/// Represents an approval action on an expense.
/// </summary>
public class ExpenseApproval
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }

    // Approval Details
    public int Level { get; set; }
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
    public string? ApproverUserId { get; set; }
    public string? ApproverName { get; set; }
    public string? ApproverRole { get; set; }

    // Decision
    public DateTime? DecisionDate { get; set; }
    public string? Comments { get; set; }
    public string? RejectionReason { get; set; }

    // Escalation
    public bool IsEscalated { get; set; }
    public DateTime? EscalatedDate { get; set; }
    public string? EscalatedTo { get; set; }
    public string? EscalationReason { get; set; }

    // Reminder
    public DateTime? ReminderSentDate { get; set; }
    public int ReminderCount { get; set; }

    // Due Date
    public DateTime? DueDate { get; set; }
    public bool IsOverdue { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Expense Expense { get; set; } = null!;
}

/// <summary>
/// Represents an attachment (receipt/document) for an expense.
/// </summary>
public class ExpenseAttachment
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }

    // File Details
    public string FileName { get; set; } = string.Empty;
    public string? OriginalFileName { get; set; }
    public string? FileUrl { get; set; }
    public string? ContentType { get; set; }
    public long FileSize { get; set; }

    // Description
    public string? Description { get; set; }
    public string? AttachmentType { get; set; } // Receipt, Invoice, Contract, etc.

    // Metadata
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string? UploadedBy { get; set; }

    // Navigation
    public Expense Expense { get; set; } = null!;
}

/// <summary>
/// Represents a payment made against an expense.
/// </summary>
public class ExpensePayment
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Guid? PaymentId { get; set; }

    // Payment Details
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public string? Reference { get; set; }

    // Bank Details
    public Guid? BankAccountId { get; set; }
    public string? ChequeNumber { get; set; }
    public string? TransactionReference { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Expense Expense { get; set; } = null!;
    public Payment? Payment { get; set; }
    public ChartOfAccount? BankAccount { get; set; }
}
